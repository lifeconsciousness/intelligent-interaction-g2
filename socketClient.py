import socket
import random

def main():
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect(('localhost', 2000))

    x = random.uniform(-1, 1)
    y = random.uniform(-1, 1)
    z = random.uniform(-1, 1)
        
    message = f"{x},{y},{z}"
    s.sendall(message.encode('utf-8'))
    
    s.close()

if __name__ == "__main__":
    main()
