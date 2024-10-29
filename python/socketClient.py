import zmq
from eye_trackers.eyetracking_mediapipe import EyeTrackingMediapipe
from interfaces.eyetracker import EyeTrackerInterface
import threading
# import time
import logging

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class SocketClient:

    def __init__(self, eye_tracking: EyeTrackerInterface, url: str = 'tcp://localhost:2000'):
        self.eye_tracking: EyeTrackerInterface = eye_tracking
        self.url: str = url
        self.context: zmq.Context = None
        self.socket: zmq.SyncSocket = None
        self._stop_event : threading.Event = threading.Event()

    def connect(self):
        self.context = zmq.Context()
        self.socket = self.context.socket(zmq.PUSH)
        self.socket.connect(self.url)
    
    def send_data(self, data: tuple):
        if data is None:
            return
        x, y, distance = data
        self.socket.send_string(f'{x},{y},{distance}')

    def main(self):
        self.connect()
        tracking_thread = threading.Thread(target=self.eye_tracking.eye_tracking)
        tracking_thread.start()

        previous_values = (0, 0, 0)

        try:
            while True:
                x, y, distance = self.eye_tracking.getPosition()
                if previous_values == (x, y, distance):
                    continue
                previous_values = (x, y, distance)
                self.send_data(previous_values)
                # time.sleep(0.1)
        except KeyboardInterrupt:
            self.eye_tracking.stop()
        except Exception as e:
            print(e)
            exit()
        finally:
            tracking_thread.join()
            self.context.term()

if __name__ == "__main__":
    eye_tracking = EyeTrackingMediapipe()
    socket_client = SocketClient(eye_tracking)
    socket_client.main()
