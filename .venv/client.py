import socket, threading, json, time
from colorama import init, Fore, Style
from datetime import datetime

init()

HOST = '127.0.0.1'
PORT = 5001
AMR_COUNT = 6
ROBOT_COUNT = 4
client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client_socket.connect((HOST, PORT))

stop_while = False

time_data = {
    "start_time": 0,
    "end_time": 0,
    "received": 0,
    "action_start": 0,
    "main_thread": 0
}

def listen():
    main_res = None
    try:
        while True:
            response = client_socket.recv(1024).decode("utf-8").strip()
            if main_res == None:
                main_res = response

            if response in ["close", "exit"]:
                global stop_while
                stop_while = True
                client_socket.close()
    
            elif response in ["start_time", "end_time", "received", "action_start", "main_thread"]:
                time_data[response] = time.time() - time_data["start_time"]
                if response == "end_time" and main_res == "TimeTest":
                    print(Fore.RED + f"\nStart Time: 0\nEnd Time / Total Time: {time_data["end_time"]}\nCommand Received by Unity: {time_data["received"]}\nThread to Main Activity: {time_data["main_thread"]}\nAction Start: {time_data["action_start"]}\n" + Style.RESET_ALL)
                    time_data["action_start"] = time_data["end_time"] = time_data["main_thread"] = time_data["received"] = time_data["start_time"] = 0
            
            elif response in ["readyToMoveBodyManually", "readyToMoveBodyAutomate", "TimeTest"]:
                t1 = threading.Thread(target=send, args=(response, ))
                t1.daemon = True
                t1.start()
            elif response == "to_start_nav":
                print(f"{datetime.now().strftime("%H:%M:%S.%f")[:-1]}: Starting NavMesh!\n")
            else:
                print(Fore.GREEN + f"\nFROM SERVER:\n{response}\n" + Style.RESET_ALL)
    except:
        client_socket.close()
        raise KeyboardInterrupt

def send(res):
    if res == "TimeTest":
        time.sleep(5)
        # threading.Thread(target=listen_input).start()
        data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.18, "id": 0}).encode('utf-8')
        time_data["start_time"] = time.time()
        client_socket.sendall(data + b"\n")
        time.sleep(10)
        data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 0}).encode('utf-8')
        time_data["start_time"] = time.time()
        client_socket.sendall(data + b"\n")

    elif res == "readyToMoveBodyAutomate":
            time.sleep(3)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 5.5, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 1}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(3)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": -90, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 1.5, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(2)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(5.5)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 5.035, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 5.5, "id": 5}).encode("utf-8")
            client_socket.send(data)
            time.sleep(5)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 5}).encode("utf-8")
            client_socket.send(data + b"\n")
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 5.5, "id": 1}).encode("utf-8")
            client_socket.send(data)
            time.sleep(4.5)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 0.45, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(2)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.145, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(3)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "body", "distance": 180, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(4)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 1}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 5, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(4.5)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 2, "id": 1}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(3)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": 90, "id": 1}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "body", "distance": 180, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.18, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(2.5)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 5.035, "id": 1}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 3, "id": 5}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(5)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 5}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.12, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 1}).encode("utf-8")
            client_socket.send(data)
            time.sleep(2)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 92, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(2.5)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 0.85, "id": 1}).encode("utf-8")
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 5.035, "id": 5}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.145, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(3)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 1}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate", "mode": "AMR", "distance": -90, "id": 5}).encode("utf-8")
            client_socket.send(data + b"\n")
            data = json.dumps({"op": "rotate", "mode": "body", "distance": 180, "id": 0}).encode("utf-8")
            client_socket.send(data)
            time.sleep(3)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(2)
            # data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 1, "id": 3}).encode('utf-8')
            # client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 4.6, "id": 1}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1)
            data = json.dumps({"op": "forward", "mode": "AMR", "distance": 0.85, "id": 5}).encode("utf-8")
            client_socket.send(data)
            time.sleep(1.5)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 0}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(4)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.18, "id": 1}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 1, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(2.5)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.145, "id": 0}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.12, "id": 1}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(2)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 0}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": -90, "id": 5}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 180, "id": 0}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 90, "id": 1}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 1}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 4.2, "id": 5}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 1}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 4, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "release" ,"mode": "body", "distance": 0, "id": 0}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 1, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(2)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 2.1, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "drop" ,"mode": "extender", "distance": 0.18, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.12, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 92, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(5.5)
            data = json.dumps({"op": "release" ,"mode": "extender", "distance": 0, "id": 3}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "lift" ,"mode": "extender", "distance": 0.01, "id": 3}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 4}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(1)
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 88, "id": 3}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 90, "id": 1}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            data = json.dumps({"op": "rotate" ,"mode": "body", "distance": 88, "id": 2}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(4.5)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 1, "id": 4}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(3.5)
            data = json.dumps({"op": "rotate" ,"mode": "AMR", "distance": 90, "id": 4}).encode('utf-8')
            client_socket.sendall(data)
            time.sleep(5.5)
            data = json.dumps({"op": "forward" ,"mode": "AMR", "distance": 3.79, "id": 4}).encode('utf-8')
            client_socket.sendall(data)

    elif res == "readyToMoveBodyManually":
        while True:
            if stop_while:
                return
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

            if stop_while:
                return
            data = json.dumps({"op": op ,"mode": mode, "distance": distance, "id": id}).encode('utf-8')
            client_socket.sendall(data + b"\n")
            print(f"{datetime.now().strftime("%H:%M:%S.%f")[:-1]}: Transmitted!\n")

listen()