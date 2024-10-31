from eyetracking import EyeTracking
import threading
import socket
import json

def main():
    # Set up UDP socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    UDP_IP = "127.0.0.1"  # Use the local loopback address or the IP of the machine running Unity/Godot
    UDP_PORT = 5005

    eye_tracking = EyeTracking()
    tracking_thread = threading.Thread(target=eye_tracking.eye_tracking)
    tracking_thread.start()

    old_values = (0, 0, 0)

    try:
        while True:
            x, y, z = eye_tracking.getPosition()
            if old_values == (x, y, z):
                continue
            old_values = (x, y, z)

            coords = {
                "x": x,
                "y": y,
                "z": z
            }

            # Send the coordinates as a JSON string over UDP
            sock.sendto(json.dumps(coords).encode(), (UDP_IP, UDP_PORT))
    except KeyboardInterrupt:
        eye_tracking.stop()
    except Exception as e:
        print(e)
        exit()
    finally:
        tracking_thread.join()

if __name__ == "__main__":
    main()
