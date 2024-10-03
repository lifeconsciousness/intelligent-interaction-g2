import socket

def main():
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect(('localhost', 2000))

    message = "Hello"
    
    s.sendall(message.encode('utf-8'))
    
    # Close the socket
    s.close()

if __name__ == "__main__":
    main()
