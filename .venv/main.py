import socket, json, time, threading

HOST = '127.0.0.1'
PORT = 5001

server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.bind((HOST, PORT))
server.listen()
AMR_COUNT = 6
ROBOT_COUNT = 4

time_data = {
    "start_time": 0,
    "end_time": 0,
    "received": 0,
    "action_start": 0
}

# Time taken by an AMR for moving 5 units at 1 speed
# Start Time: 0
# End Time / Total Time: 5.002260684967041
# Command Received by Unity: 0.0020134449005126953
# Action Start: 0.0029659271240234375

# Time taken by an AMR for moving 90deg at 20 speed
# Start Time: 0
# End Time / Total Time: 4.504694700241089
# Command Received by Unity: 0.002542734146118164
# Action Start: 0.0035943984985351562

# Time taken by Picker to rotate 90deg at 20 speed
# Start Time: 0
# End Time / Total Time: 4.505250692367554
# Command Received by Unity: 0.003449678421020508
# Action Start: 0.004575490951538086

# Time taken by Picker to drop 0.1 units at 20f
# Start Time: 0
# End Time / Total Time: 1.8023569583892822
# Command Received by Unity: 0.001739501953125
# Action Start: 0.002817392349243164

# Time taken by Picker to lift 0.1 units at 20f
# Start Time: 0
# End Time / Total Time: 2.0034704208374023
# Command Received by Unity: 0.0015361309051513672
# Action Start: 0.0017313957214355469

# Start Time: 0
# End Time / Total Time: 4.509523868560791
# Command Received by Unity: 0.0073757171630859375
# Action Start: 1740139159.0430486

def listen_input():
    close = input()
    if close in ["close", "exit"]:
        conn.close()
        server.close()
        raise KeyboardInterrupt

print(f"Listening on {HOST}:{PORT}")
conn, addr = server.accept()
print(f"Connected by {addr}")

def listen_to_movements():
    print("STARTED LISTENING RESPONSE TIME:")
    while True:
        data = conn.recv(1024).decode('utf-8').strip()
        time_data[data] = time.time() - time_data["start_time"]
        if data == "end_time":
            print(f"\nStart Time: 0\nEnd Time / Total Time: {time_data["end_time"]}\nCommand Received by Unity: {time_data["received"]}\nAction Start: {time_data["start_time"]}\n")

try:
    # while True:
        received = conn.recv(1024).decode('utf-8').strip()
        print(received)
        if not received:
            # continue
            None

        # if received not in ["readyToMoveBodyAutomate", "readyToMoveBodyManually"]:
        t1 = threading.Thread(target=listen_to_movements)
        t1.start()

        if received == "TimeTest":
            time.sleep(5)
            threading.Thread(target=listen_input).start()
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 90, "id": 0}).encode('utf-8')
            time_data["start_time"] = time.time()
            conn.sendall(data)
            time.sleep(5)
            while True:
                data = conn.recv(1024).decode('utf-8').strip()
                time_data[data] = time.time() - time_data["start_time"]
                if data == "end_time":
                    # time_data["start_time"] = 0
                    print(f"\nStart Time: 0\nEnd Time / Total Time: {time_data["end_time"]}\nCommand Received by Unity: {time_data["received"]}\nAction Start: {time_data["action_start"]}\n")

        elif received == "readyToMoveBodyAutomate":
            time.sleep(3)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 5.5, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 1}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(3)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": -90, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 1.5, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(2)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(5.5)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 5.035, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 5.5, "id": 5}).encode("utf-8")
            conn.send(data)
            time.sleep(5)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 5}).encode("utf-8")
            conn.send(data + b"\n")
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 5.5, "id": 1}).encode("utf-8")
            conn.send(data)
            time.sleep(4.5)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 0.45, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(2)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.145, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(3)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "body", "distance": 180, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(4)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 1}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 5, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(4.5)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 2, "id": 1}).encode('utf-8')
            conn.sendall(data)
            time.sleep(3)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 1}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "body", "distance": 180, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.18, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(2.5)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 5.035, "id": 1}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 3, "id": 5}).encode('utf-8')
            conn.sendall(data)
            time.sleep(5)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 5}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.12, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 1}).encode("utf-8")
            conn.send(data)
            time.sleep(2)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 92, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(2.5)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 0.85, "id": 1}).encode("utf-8")
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 5.035, "id": 5}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.145, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(3)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 1}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 5}).encode("utf-8")
            conn.send(data + b"\n")
            data = json.dumps({"op": "rotate", "mode": "body", "distance": 180, "id": 0}).encode("utf-8")
            conn.send(data)
            time.sleep(3)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(2)
            # data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 1, "id": 3}).encode('utf-8')
            # conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 4.6, "id": 1}).encode("utf-8")
            conn.send(data)
            time.sleep(1)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 0.85, "id": 5}).encode("utf-8")
            conn.send(data)
            time.sleep(1.5)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 0}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(4)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.18, "id": 1}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 1, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(2.5)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.145, "id": 0}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.12, "id": 1}).encode('utf-8')
            conn.sendall(data)
            time.sleep(2)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 0}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": -90, "id": 5}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 0}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 90, "id": 1}).encode('utf-8')
            conn.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 1}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 4.2, "id": 5}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 1}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 4, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "release" ,"mode": "body", "distance": 0, "id": 0}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 1, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(2)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 2.1, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.18, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.12, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 92, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(5.5)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 3}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 3}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 4}).encode('utf-8')
            conn.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 88, "id": 3}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 90, "id": 1}).encode('utf-8')
            conn.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 88, "id": 2}).encode('utf-8')
            conn.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 1, "id": 4}).encode('utf-8')
            conn.sendall(data)
            time.sleep(3.5)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 4}).encode('utf-8')
            conn.sendall(data)
            time.sleep(5.5)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 3.79, "id": 4}).encode('utf-8')
            conn.sendall(data)

        elif received == "readyToMoveBodyManually":
            while True:
                op = input("Enter the operation to perform(op): ").lower()
                if op not in ["rotate", "drop", "lift", "release", "forward"]:
                    print("Invalid operation entered.\n")
                    continue
                else:
                    mode = input("Enter the part to perform the operation on(mode): ")
                    # Body & AMR has 1 operation - ["rotate"]
                    if op in ["rotate"] and mode in ["body", "AMR"]:
                        None
                    # AMR has 1 operation - ["forward"]
                    elif op in ["forward"] and mode in ["AMR"]:
                        None
                    # Extender has 3 operations - ["drop", "lift", "release"]
                    elif op in ["drop", "lift", "release"] and mode in ["extender"]:
                        None
                    else:
                        print(f"Part '{mode}' has no operation '{op}' yet.\n")
                        continue
                try:
                    distance = float(input("Enter the distance/angle to move/rotate(distance): "))
                except:
                    print("Invalid number entered.\n")
                    continue
                finally:
                    try:
                        id = int(input("Enter the ID(id): "))
                        if mode == "AMR" and id < AMR_COUNT and id >= 0:
                            None
                        elif mode in ["body", "extender"] and id < ROBOT_COUNT and id >= 0:
                            None
                        else: raise Exception
                    except:
                        print("Invalid ID entered.")
                        continue
                    finally:
                        data = json.dumps({"op": op ,"mode": mode, "distance": distance, "id": id}).encode('utf-8')
                        conn.sendall(data + b"\n")
                        print("Transmitted!\n")
                    
            
except KeyboardInterrupt:
    print("Closing server")
finally:
    conn.close()
    server.close()
