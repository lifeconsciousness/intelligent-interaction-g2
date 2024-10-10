import zmq
from eyetracking import EyeTracking
import threading
import time

def main():
    context = zmq.Context()
    socket = context.socket(zmq.PUSH)
    socket.connect('tcp://localhost:2000')

    eye_tracking = EyeTracking()
    tracking_thread = threading.Thread(target=eye_tracking.eye_tracking)
    tracking_thread.start()

    old_values = (0, 0, 0)

    try:
        while True:
            x, y, distance = eye_tracking.getPosition()
            if old_values == (x, y, distance):
                continue
            old_values = (x, y, distance)
            #print(x, y, distance)
            socket.send_string(f'{x},{y},{distance}')
            #time.sleep(0.1)
    except KeyboardInterrupt:
        eye_tracking.stop()
    except Exception as e:
        print(e)
        exit()
    finally:
        tracking_thread.join()
        context.term()

if __name__ == "__main__":
    main()
