import cv2
import time
import sys
import os
from argparse import ArgumentParser
import abc

class EyeTracker(abc.ABC):
    @abc.abstractmethod
    def eye_tracking(self, frame):
        pass

class Mediapipe(EyeTracker):
    def __init__(self):
        parent_dir = os.path.abspath(os.path.join(os.path.dirname(__file__), '..'))
        sys.path.append(parent_dir)
        from eye_trackers.eyetracking_mediapipe import EyeTrackingMediapipe
        self.tracker = EyeTrackingMediapipe()

    def eye_tracking(self, frame):
        return self.tracker.eye_tracking(frame)

class Haarcascade(EyeTracker):
    def __init__(self):
        parent_dir = os.path.abspath(os.path.join(os.path.dirname(__file__), '..'))
        sys.path.append(parent_dir)
        from eye_trackers.eyetracking_haarcascade import EyeTrackingHaarcascade

        self.tracker = EyeTrackingHaarcascade()

    def eye_tracking(self, frame):
        return self.tracker.eye_tracking(frame)

def run(tracker : EyeTracker):
    cap = cv2.VideoCapture(0)
    frame_count = 0
    start_time = time.time()
    latencies = []
    missed_frames = 0

    while time.time() < start_time + 30: 
        frame_start_time = time.time()
        ret, frame = cap.read()

        if not ret:
            break

        tracking_result = tracker.eye_tracking(frame)

        if tracking_result and tracking_result.eye_rect:
            x, y, width, height = tracking_result.eye_rect
            cv2.rectangle(frame, (int(x), int(y)), (int(x + width), int(y + height)), (0, 255, 0), 2)
        else:
            missed_frames += 1

        frame_end_time = time.time()
        latencies.append(frame_end_time - frame_start_time)
        frame_count += 1

        cv2.imshow('Eye Tracking Metrics', frame)
        if cv2.waitKey(1) & 0xFF == 27: 
            break

    cap.release()
    cv2.destroyAllWindows()

    return frame_count, latencies, missed_frames, start_time

if __name__ == "__main__":
    parser = ArgumentParser()
    parser.add_argument("--mediapipe", action="store_true", help="Use mediapipe for eye tracking")
    parser.add_argument("--haarcascade", action="store_true", help="Use haarcascade for eye tracking")
    args = parser.parse_args()

    if args.mediapipe:
        tracker = Mediapipe()
    elif args.haarcascade:
        tracker = Haarcascade()
    else:
        print("Error: Please specify an eye tracker to use.")
        exit(1)

    frame_count, latencies, missed_frames, start_time = run(tracker)

    from display_metrics import display_metrics, Metrics

    metrics = Metrics(
        latencies=latencies,
        frame_count=frame_count,
        miss_rate=(missed_frames / frame_count) * 100,
        fps=frame_count / (time.time() - start_time)
    )

    display_metrics(metrics)