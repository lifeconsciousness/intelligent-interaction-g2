from eye_trackers.eyetracking_mediapipe import EyeTrackingMediapipe
from socketClient import SocketClient

if __name__ == "__main__":
    eye_tracking = EyeTrackingMediapipe()
    socket_client = SocketClient(eye_tracking)
    socket_client.main()