import cv2
import os

from eye_trackers.eyetracker import EyeTrackerInterface, EyeTrackingResult

base_dir = os.path.dirname(__file__)

eye_cascade_path = os.path.join(base_dir, "haarcascade/haarcascade_eye.xml")
face_cascade_path = os.path.join(base_dir, "haarcascade/haarcascade_frontalface_default.xml")

eye_cascade = cv2.CascadeClassifier(eye_cascade_path)
face_cascade = cv2.CascadeClassifier(face_cascade_path)


class EyeTrackingHaarcascade(EyeTrackerInterface):

    def __init__(self):
        super().__init__()

    def eye_tracking(self, image):
        gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        faces = face_cascade.detectMultiScale(gray, 1.3, 5)

        for (x, y, w, h) in faces:
            roi_gray = gray[y:y + h, x:x + w]

            eyes = eye_cascade.detectMultiScale(roi_gray, 1.1, 5)
            eyes = sorted(eyes, key=lambda e: e[0], reverse=True) 
            right_eye = eyes[0] if len(eyes) > 0 else None

            if right_eye is not None:
                (ex, ey, ew, eh) = right_eye

                self.x_position = x + ex + ew / 2
                self.y_position = y + ey + eh / 2
                self.distance = self.calculate_distance(ew)
                self.eye_rect = (x, y, ew, eh)
                
                return EyeTrackingResult(
                    x=x + ex + ew / 2,
                    y=y + ey + eh / 2,
                    distance=self.calculate_distance(ew),
                    eye_rect=(x, y, ew, eh)
                )
        return EyeTrackingResult(
            x=self.x_position,
            y=self.y_position,
            distance=self.distance,
            eye_rect=self.eye_rect
        )
