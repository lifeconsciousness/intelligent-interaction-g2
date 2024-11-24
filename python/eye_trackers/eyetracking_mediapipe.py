import cv2
import mediapipe as mp
from eye_trackers.eyetracker import EyeTrackerInterface, EyeTrackingResult

class EyeTrackingMediaPipeConfig:
    RIGHT_EYE_OUTER = 263
    RIGHT_EYE_INNER = 362
    RIGHT_EYE_TOP = 386
    RIGHT_EYE_BOTTOM = 374

class EyeTrackingMediapipe(EyeTrackerInterface):
    def __init__(self):
        super().__init__()
        self.mp_face_mesh = mp.solutions.face_mesh
        self.face_mesh = self.mp_face_mesh.FaceMesh(static_image_mode=False, max_num_faces=1)
        self.mp_drawing = mp.solutions.drawing_utils
        self.landmarks = None

    def get_right_eye_rect(self, image):
        eye_top = int(self.landmarks[EyeTrackingMediaPipeConfig.RIGHT_EYE_TOP].y * image.shape[0])
        eye_left = int(self.landmarks[EyeTrackingMediaPipeConfig.RIGHT_EYE_INNER].x * image.shape[1])
        eye_bottom = int(self.landmarks[EyeTrackingMediaPipeConfig.RIGHT_EYE_BOTTOM].y * image.shape[0])
        eye_right = int(self.landmarks[EyeTrackingMediaPipeConfig.RIGHT_EYE_OUTER].x * image.shape[1])

        eye_top = max(0, eye_top)
        eye_left = max(0, eye_left)
        eye_bottom = min(image.shape[0], eye_bottom)
        eye_right = min(image.shape[1], eye_right)

        width = eye_right - eye_left
        height = eye_bottom - eye_top
        return eye_left, eye_top, width, height

    def eye_tracking(self, image):
        image_rgb = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        results = self.face_mesh.process(image_rgb)

        if results.multi_face_landmarks:
            for face_landmarks in results.multi_face_landmarks:
                self.landmarks = face_landmarks.landmark
                x_right_eye, y_right_eye, right_eye_width, right_eye_height = self.get_right_eye_rect(image)
                distance = self.calculate_distance(right_eye_width)

                
                self.x_position = x_right_eye + right_eye_width / 2
                self.y_position = y_right_eye + right_eye_height / 2
                self.distance = distance

                self.eye_rect = (x_right_eye, y_right_eye, right_eye_width, right_eye_height) 

                return EyeTrackingResult(
                    x=self.x_position,
                    y=self.y_position,
                    distance=self.distance,
                    eye_rect=self.eye_rect
                )
        else:
            return EyeTrackingResult(
                x=self.x_position,
                y=self.y_position,
                distance=self.distance,
                eye_rect=self.eye_rect
            )
