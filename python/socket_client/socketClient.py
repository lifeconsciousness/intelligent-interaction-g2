import logging
import json

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)
import socket

class SocketClientConfig: 
    UDP_IP = "127.0.0.1"
    UDP_PORT = 5005

class SocketClient:

    def __init__(self):
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.config = SocketClientConfig()
        
    def send_data(self, data: tuple):
        coordiantes = {
            "x": data[0],
            "y": data[1],
            "distance": data[2]
        }
        self.sock.sendto(json.dumps(coordiantes).encode(), (self.config.UDP_IP, self.config.UDP_PORT))
