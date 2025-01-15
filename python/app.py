import cv2

from eye_trackers.eyetracking_mediapipe import EyeTrackingMediapipe
from eye_trackers.eyetracker import EyeTrackerInterface
from eye_trackers.eyetracking_haarcascade import EyeTrackingHaarcascade
from socket_client.socketClient import SocketClient

from argparse import ArgumentParser
import threading

def main(eye_tracking: EyeTrackerInterface, socket_client = None):
    tracking_thread = threading.Thread(target=eye_tracking.eye_tracking)
    tracking_thread.start()
    
    x = 0
    y = 0
    distance = 0
    
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
                
                if results is not None:
                    x = results.x
                    y = results.y
                    distance = results.distance

                if args.verbose:
                    print(f"X: {x}, Y: {y}, Distance: {distance:.2f}")

                if socket_client is not None:
                    socket_client.send_data((x, y, distance))

                if results is not None and results.eye_rect is not None and args.video:
                    x_rect, y_rect, w_rect, h_rect = results.eye_rect
                    cv2.rectangle(image, (int(x_rect), int(y_rect)), (int(x_rect + w_rect), int(y_rect + h_rect)), (255, 0, 0), 2)

                if args.video:
                    cv2.imshow("Eye Tracking", cv2.flip(image, 1))
                if cv2.waitKey(5) & 0xFF == 27: 
                    break

            cap.release()
            cv2.destroyAllWindows()

        except KeyboardInterrupt:
            print("Stopping eye tracking.")
            cap.release()
            tracking_thread.join()
            cv2.destroyAllWindows()

if __name__ == "__main__":
    
    parser = ArgumentParser()

    parser.add_argument("--mediapipe", action="store_true", help="Use mediapipe for eye tracking")
    parser.add_argument("--haarcascade", action="store_true", help="Use haarcascade for eye tracking")
    parser.add_argument("--video", action="store_true", help="Display Eye Tracking on video")
    parser.add_argument("--verbose", action="store_true", help="Display verbose output")
    args = parser.parse_args()
    
    eye_tracking: EyeTrackerInterface = None
    if args.mediapipe:
        eye_tracking = EyeTrackingMediapipe()
    elif args.haarcascade:
        eye_tracking = EyeTrackingHaarcascade()
    else:
        print("Error: Please specify an eye tracker to use.")
        exit(1)

    socket_client = SocketClient()
    main(eye_tracking, socket_client)