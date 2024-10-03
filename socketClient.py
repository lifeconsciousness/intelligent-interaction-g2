import socket
from eyetracking import EyeTracking
import threading
import time

def main():
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect(('localhost', 2000))

    eye_tracking = EyeTracking()
    tracking_thread = threading.Thread(target=eye_tracking.eye_tracking)
    tracking_thread.start()

    try:
        while not eye_tracking.stop_event.is_set():
            x, y, distance = eye_tracking.getPosition()
            print(x, y, distance)
            s.sendall(f'{x} {y} {distance}'.encode())
            time.sleep(0.1) 
    except KeyboardInterrupt:
        eye_tracking.stop()
    finally:
        tracking_thread.join()
        s.close()

if __name__ == "__main__":
    main()
