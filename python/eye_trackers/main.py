import cv2
from eyetracking_mediapipe import EyeTrackingMediapipe
from eyetracking_haarcascade import EyeTrackingHaarcascade
from eyetracker import EyeTrackerInterface

from argparse import ArgumentParser

parser = ArgumentParser()

parser.add_argument("--mediapipe", action="store_true", help="Use mediapipe for eye tracking")
parser.add_argument("--haarcascade", action="store_true", help="Use haarcascade for eye tracking")
args = parser.parse_args()

if __name__ == "__main__":
    eye_tracking: EyeTrackerInterface = None
    if args.mediapipe:
        eye_tracking = EyeTrackingMediapipe()
    elif args.haarcascade:
        eye_tracking = EyeTrackingHaarcascade()
    else:
        print("Error: Please specify an eye tracker to use.")
        exit(1)

    cap = cv2.VideoCapture(0)
    if not cap.isOpened():
        print("Error: Unable to open camera.")
    else:
        try:
            while cap.isOpened():
                success, image = cap.read()
                if not success:
                    print("Error: Could not read frame.")
                    continue

                results = eye_tracking.eye_tracking(image)
                x = results.x
                y = results.y
                distance = results.distance

                print(f"X: {x}, Y: {y}, Distance: {distance:.2f}")

                if results.eye_rect is not None:
                    x_rect, y_rect, w_rect, h_rect = results.eye_rect
                    cv2.rectangle(image, (int(x_rect), int(y_rect)), (int(x_rect + w_rect), int(y_rect + h_rect)), (255, 0, 0), 2)

                cv2.imshow("Eye Tracking", cv2.flip(image, 1))
                if cv2.waitKey(5) & 0xFF == 27: 
                    break

            cap.release()
            cv2.destroyAllWindows()

        except KeyboardInterrupt:
            print("Stopping eye tracking.")
            cap.release()
            cv2.destroyAllWindows()