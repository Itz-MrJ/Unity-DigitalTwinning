using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;

public class RobotInstance : MonoBehaviour
{
	public static RobotInstance RIM;
	private List<BodyController> MainBodyInstance = new List<BodyController>(new BodyController[10]);
	private List<ExtenderController> MainExtenderInstance = new List<ExtenderController>(new ExtenderController[10]);
	private List<float> MainExtenderMaxLength = new List<float>() { 0.19f, 0.19f, 0.19f, 0.19f, 0.19f, 0.19f, 0.19f, 0.19f, 0.19f, 0.19f };
	private List<SphereDetector> MainSphereInstance = new List<SphereDetector>(new SphereDetector[10]);

	private TcpClient client, listener;
	private NetworkStream stream, listenerStream;
	private byte[] data = new byte[1024];
	private string IP = "127.0.0.1";
	private int port = 5001, listenerPort =  5002;
	private string movement = "readyToMoveBodyManually";
	// private string movement = "TimeTest";
	private bool btnClicked = false;

	private void Awake()
	{
		if (RIM == null) RIM = this;
		else Destroy(gameObject);
	}

	void Start()
	{
		pingServer();
	}

	private async void pingServer()
	{
		try
		{
			client = new TcpClient();
			await client.ConnectAsync(IP, port);
			stream = client.GetStream();
			Debug.Log("Connected to server & sending " + movement);
			GetData();
			SendCommand(movement, "client");

			return;
			listener = new TcpClient();
			await listener.ConnectAsync(IP, listenerPort);
			listenerStream = listener.GetStream();
			Debug.Log("Connected to server");
		}
		catch (Exception e)
		{
			Debug.LogError($"Connection error: {e.Message}");
		}
	}

	[Serializable]
	private class Wrapper
	{
		public string op;
		public string mode;
		public float distance;
		public int id;
	}
	public async void SendCommand(string command, string dest)
	{
		// if (btnClicked) return;
		// btnClicked = true;
		if(dest == "client"){
			if (client.Connected)
			{
				byte[] data = Encoding.UTF8.GetBytes(command + "\n"); // Add newline for proper parsing
				await stream.WriteAsync(data, 0, data.Length);
				Debug.Log($"Sent to server {command}");
			}
		}else{
			return;
			if (listener.Connected)
			{
				byte[] data = Encoding.UTF8.GetBytes(command + "\n"); // Add newline for proper parsing
				await listenerStream.WriteAsync(data, 0, data.Length);
				Debug.Log($"Sent to listener {command}");
			}
		}
	}
	private void HandleInput(string json)
	{
		Wrapper wrap = JsonUtility.FromJson<Wrapper>(json);

		SendCommand("action_start", "client");
		if (wrap.op == "forward") AMRManager.AMRIM.GetAMR(wrap.id).MoveForward(wrap.id, wrap.distance);
		else if (wrap.op == "rotate")
		{
			if (90f != Math.Abs(wrap.distance) && wrap.mode == "AMR") return;
			if (wrap.mode == "AMR")
			{
				AMRManager.AMRIM.GetAMR(wrap.id).Rotate(wrap.id, wrap.distance);
			}
			// Body of the Robot
			else if (wrap.mode == "body")
			{
				GetBody(wrap.id).handleMoveBody(wrap.id, wrap.distance);
			}
		}
		else if (wrap.op == "drop")
		{
			GetExtender(wrap.id).DropExtender(wrap.distance, wrap.id);
		}
		else if (wrap.op == "lift")
		{
			GetExtender(wrap.id).LiftExtender(wrap.distance, wrap.id);
		}
		else if (wrap.op == "release")
		{
			GetSphere(wrap.id).Release(wrap.id);
		}
	}
	private async void GetData()
	{
		while (client.Connected)
		{
			try
			{
				int bytesRead = await stream.ReadAsync(data, 0, data.Length);
				SendCommand("received", "client");
				if (bytesRead > 0)
				{
					string json = Encoding.UTF8.GetString(data, 0, bytesRead).Trim();
					string[] parts;
					Debug.Log($"Received: {json}");
					parts = json.Split("\n");
					if (parts.Length > 1)
					{
						for (int i = 0; i < parts.Length; i++)
						{
							HandleInput(parts[i]);
						}
					}
					else HandleInput(json);
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Read error: {e.Message}");
				break;
			}
		}
	}

	public void AddBody(int i, BodyController bc)
	{
		MainBodyInstance[i] = bc;
	}

	public void AddExtender(int i, ExtenderController ec)
	{
		MainExtenderInstance[i] = ec;
	}

	public void AddSphere(int i, SphereDetector sd)
	{
		MainSphereInstance[i] = sd;
	}

	public BodyController GetBody(int i)
	{
		return MainBodyInstance[i];
	}

	public ExtenderController GetExtender(int i)
	{
		return MainExtenderInstance[i];
	}

	public SphereDetector GetSphere(int i)
	{
		return MainSphereInstance[i];
	}

	public float GetMainExtenderMaxLength(int id) {
		return MainExtenderMaxLength[id];
	}
}