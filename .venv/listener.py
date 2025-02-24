import socket, json, time, threading

HOST = '127.0.0.1'
PORT = 5002

server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.bind((HOST, PORT))
server.listen()
print(f"Listening on {HOST}:{PORT}")

def listen_input():
    close = input()
    if close in ["close", "exit"]:
        conn.close()
        server.close()
        raise KeyboardInterrupt
    
try:
    conn, addr = server.accept()
    print(f"Connected by {addr}")
    print("STARTED LISTENING")
    threading.Thread(target=listen_input).start()
    while True:
        new_movement = conn.recv(1024).decode('utf-8').strip()
        print("NEW MOVEMENT DETECTED:\n" + new_movement)

except KeyboardInterrupt:
    print("Keyboard interrupt received. Closing server...")


print("Server closed.")