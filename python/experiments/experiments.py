import cv2
import time
import numpy as np
from mediapipes import EyeTrackingMediapipe
import os
from argparse import ArgumentParser

parser = ArgumentParser()
## add video argument in mp4 format
# parser.add_argument("--video", type=str, help="Path to video file")
parser.add_argument("--mediapipe", action="store_true", help="Use mediapipe for eye tracking")
parser.add_argument("--haarcascade", action="store_true", help="Use haarcascade for eye tracking")
args = parser.parse_args()

base_dir = os.path.dirname(__file__)
eye_cascade_path = os.path.join(base_dir, "haarcascade/haarcascade_eye.xml")
face_cascade_path = os.path.join(base_dir, "haarcascade/haarcascade_frontalface_default.xml")

eye_cascade = cv2.CascadeClassifier(eye_cascade_path)
face_cascade = cv2.CascadeClassifier(face_cascade_path)

eye_tracker = EyeTrackingMediapipe()

frame_count = 0
start_time = time.time()
latencies = []
missed_frames = 0

cap = cv2.VideoCapture(0)

def mediapipe():
    global frame_count, start_time, latencies, missed_frames, jitter_list
    while time.time() < start_time + 5:
        frame_start_time = time.time()
        ret, frame = cap.read()

        if not ret:
            break 

        tracking_result = eye_tracker.eye_tracking(frame)

        if tracking_result and tracking_result.eye_rect:
            x, y, width, height = tracking_result.eye_rect
            cv2.rectangle(frame, (int(x), int(y)), (int(x + width), int(y + height)), (0, 255, 0), 2)
        else:
            missed_frames += 1

        frame_end_time = time.time()
        frame_latency = frame_end_time - frame_start_time
        latencies.append(frame_latency)

        if len(latencies) > 1:
            jitter = abs(latencies[-1] - latencies[-2])
            jitter_list.append(jitter)

        frame_count += 1

        cv2.imshow('Eye Tracking Metrics', frame)

        k = cv2.waitKey(1) & 0xff
        if k == 27: 
            break

    cap.release()
    cv2.destroyAllWindows()
    
def haarcascade():
    global frame_count, start_time, latencies, missed_frames, jitter_list
    while time.time() < start_time + 30:
        frame_start_time = time.time()
        ret, img = cap.read()

        if not ret:
            break 

        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        faces = face_cascade.detectMultiScale(gray, 1.3, 5)

        found_eye = False
        for (x, y, w, h) in faces:
            cv2.rectangle(img, (x, y), (x + w, y + h), (255, 0, 0), 2)
            roi_gray = gray[y:y + h, x:x + w]
            roi_color = img[y:y + h, x:x + w]

            eyes = eye_cascade.detectMultiScale(roi_gray, scaleFactor=1.1, minNeighbors=50, minSize=(90, 90))
            eyes = sorted(eyes, key=lambda e: e[0])

            if len(eyes) > 0:
                found_eye = True
                (ex, ey, ew, eh) = eyes[0] 

                cv2.rectangle(roi_color, (ex, ey), (ex + ew, ey + eh), (0, 255, 0), 2)

        frame_end_time = time.time()
        frame_latency = frame_end_time - frame_start_time
        latencies.append(frame_latency)

        if not found_eye:
            missed_frames += 1

        frame_count += 1

        cv2.imshow('Eye Tracking Metrics', img)
        k = cv2.waitKey(30) & 0xff
        if k == 27:
            break

    cap.release()
    cv2.destroyAllWindows()

if __name__ == "__main__":
    if args.mediapipe:
        mediapipe()
    elif args.haarcascade:
        haarcascade()
    else:
        print("Error: Please specify an eye tracker to use.")
        exit(1)

    from display_metrics import display_metrics, Metrics

    metrics = Metrics(
        latencies=latencies,
        frame_count=frame_count,
        miss_rate=(missed_frames / frame_count) * 100,
        fps=frame_count / (time.time() - start_time)
    )

    display_metrics(metrics)