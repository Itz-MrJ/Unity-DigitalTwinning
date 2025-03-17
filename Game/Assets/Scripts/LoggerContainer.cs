using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoggerContainer : MonoBehaviour
{
    public static LoggerContainer LCI;
    public TMP_Text logs; 
    public GameObject button;
    private Button btn;
    private float speed = 100f;
    private Text t;
    void Awake(){
        if(LCI)Destroy(gameObject);
        else LCI = this;
    }
    public void AddColoredLine(string text, string color)
    {
        // string colorHex = ColorUtility.ToHtmlStringRGB(color);
        logs.text += $"<color=#{color}>{text}</color>\n";
    }
    void handleToggler(){
        if (t.text == "Open") {
            FreeCamera.FCI.UI = true;
            StartCoroutine(ToForward(0, false));
            t.text = "Close";
        }
        else {
            FreeCamera.FCI.UI = false;
            StartCoroutine(ToForward(-225, true));
            t.text = "Open";
        }     
    }
    void Start()
    {
        btn = button.GetComponent<Button>();
        t = button.transform.GetChild(0).gameObject.GetComponent<Text>();
        btn.onClick.AddListener(handleToggler);
        StartCoroutine(ToForward(-225, true));
    }

    IEnumerator ToForward(float distance, bool down)
    {
        float transformPos = transform.position.y;
        if (down)
            while (distance < transformPos)
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
                transformPos = transform.position.y;
                yield return null;
            }
        else
            while (distance > transformPos)
            {
                transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                transformPos = transform.position.y;
                yield return null;
            }
        // RobotInstance.RIM.SendCommand("end_time", "client");
        transform.position = new Vector3(transform.position.x, distance, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
