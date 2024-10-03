import cv2
import mediapipe as mp
import threading
import time

mp_face_mesh = mp.solutions.face_mesh
mp_drawing = mp.solutions.drawing_utils
face_mesh = mp_face_mesh.FaceMesh(static_image_mode=False, max_num_faces=1)

RIGHT_EYE_OUTER = 263
RIGHT_EYE_INNER = 362
RIGHT_EYE_TOP = 257
RIGHT_EYE_BOTTOM = 374

KNOWN_DISTANCE = 76.2  
KNOWN_FACE_WIDTH = 3.0  
FOCAL_LENGTH = 381.0  

class EyeTracking:

    def __init__ (self):
        self.x_position = 0
        self.y_position = 0
        self.distance = 0
        self.stop_event = threading.Event() 

    def getRightEyeRect(self, image, landmarks):
        eye_top = int(landmarks[RIGHT_EYE_TOP].y * image.shape[0])
        eye_left = int(landmarks[RIGHT_EYE_INNER].x * image.shape[1])
        eye_bottom = int(landmarks[RIGHT_EYE_BOTTOM].y * image.shape[0])
        eye_right = int(landmarks[RIGHT_EYE_OUTER].x * image.shape[1])

        eye_top = max(0, eye_top)
        eye_left = max(0, eye_left)
        eye_bottom = min(image.shape[0], eye_bottom)
        eye_right = min(image.shape[1], eye_right)

        width = eye_right - eye_left
        height = eye_bottom - eye_top
        return eye_left, eye_top, width, height

    def focal_length_finder(self, measured_distance, real_width, width_in_rf_image):
        focal_length_value = (width_in_rf_image * measured_distance) / real_width
        return focal_length_value

    def distance_finder(self, focal_length, real_face_width, face_width_in_frame):
        distance = (real_face_width * focal_length) / face_width_in_frame
        return distance

    def eye_tracking(self):
        cap = cv2.VideoCapture(0)
        while cap.isOpened() and not self.stop_event.is_set():
            success, image = cap.read()
            if not success:
                continue

            image_rgb = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
            results = face_mesh.process(image_rgb)

            if results.multi_face_landmarks:
                for face_landmarks in results.multi_face_landmarks:
                    x_right_eye, y_right_eye, right_eye_width, right_eye_height = self.getRightEyeRect(image, face_landmarks.landmark)
                    distance = self.distance_finder(FOCAL_LENGTH, KNOWN_FACE_WIDTH, right_eye_width)

                    self.x_position = x_right_eye + right_eye_width / 2
                    self.y_position = y_right_eye + right_eye_height / 2
                    self.distance = distance
            time.sleep(0.01)

        cap.release()

    def getPosition(self):
        return self.x_position, self.y_position, self.distance

    def stop(self):
        self.stop_event.set()

if __name__ == "__main__":
    eye_tracking = EyeTracking()
    tracking_thread = threading.Thread(target=eye_tracking.eye_tracking)
    tracking_thread.start()

    try:
        while not eye_tracking.stop_event.is_set():
            x, y, distance = eye_tracking.getPosition()
            time.sleep(0.1) 
    except KeyboardInterrupt:
        eye_tracking.stop()
    finally:
        tracking_thread.join()