using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using System.Text;
using UnityEngine.UI;

public class BodyController : MonoBehaviour
{
    public int ID;
    public GameObject body, AMR;
    private float speed = 20f;
    private Quaternion bodyPosition;
    private bool isMoving = false;
    private TcpClient client;
    private NetworkStream stream;
    private byte[] data = new byte[1024];
    private string IP = "127.0.0.1";
    private int port = 5001;
    private bool btnClicked = false;
    // Optional input - readyToMoveBodyManually / readyToMoveBodyAutomate
    public string movement = "readyToMoveBodyManually";
    // void Awake()
    // {
    // }

    // Start is called before the first frame update
    void Start()
    {
        RobotInstance.RIM.AddBody(ID, this);
    }
    public void handleMoveBody(int id, float y){
        if(id != ID) return;
        Debug.Log("Moving in Body!");
        bodyPosition = Quaternion.Euler(body.transform.eulerAngles.x, body.transform.eulerAngles.y + y, body.transform.eulerAngles.z);
        StartCoroutine(MoveBody());
    }

    IEnumerator MoveBody()
    {
        isMoving = true;
        while (Quaternion.Angle(body.transform.rotation, bodyPosition) > 0.01f)
        {
            body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, bodyPosition, speed * Time.deltaTime);
            yield return null;
        }
        body.transform.rotation = bodyPosition;
        isMoving = false;
    }

    private void OnApplicationQuit()
    {
        stream?.Close();
        client?.Close();
    }

    [Serializable]
    private class Wrapper
    {
        public string op;
        public string mode;
        public float distance;
    }

}
