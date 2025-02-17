using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtenderController : MonoBehaviour{
    // Start is called before the first frame update
    public int ID;
    // public static ExtenderController Instance;
    public GameObject extender, hand;
    private float minY, maxY, fixedMinY = 0.01f, fixedMaxY;
    private float speed = 0.05f, lastDrop=0.0f;
    private Vector3 extenderScale, extenderPosition, originalHand, handPos;
    void Start(){
        RobotInstance.RIM.AddExtender(ID, this);
        fixedMaxY = RobotInstance.RIM.MainExtenderMaxLength[ID];
        extenderPosition = extender.transform.localPosition;
        extenderScale = extender.transform.localScale;

        originalHand = hand.transform.lossyScale;
        handPos = hand.transform.localPosition;
    }

    public void LiftExtender(float units, int id){
        if(id != ID)return;
        if(units < fixedMinY)Debug.Log("Cannot lift to that position.");
        else if(units >= lastDrop)Debug.Log("Wants to lift extender using `lift`.");
        else{
            lastDrop = units;
            minY = units;
            extenderScale = extender.transform.localScale;
            StartCoroutine(ScaleUp());
        }
    }

    IEnumerator ScaleUp(){
        while(extender.transform.localScale.y > minY){
            extender.transform.localScale -= new Vector3(0, speed * Time.deltaTime, 0);
            extender.transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
            hand.transform.localPosition += new Vector3(0, speed * Time.deltaTime * 2, 0);
            yield return null;
        }
        extender.transform.localScale = new Vector3(extender.transform.localScale.x, minY, extender.transform.localScale.z);
    }

    public void DropExtender(float units, int id){
        Debug.Log($"About to Drop!! {units} - {id}");
        if(id != ID)return;
        if(units > fixedMaxY)Debug.Log("Cannot drop to that position.");
        else if(units <= lastDrop)Debug.Log("Wants to reduce extender height using `drop`.");
        else{
            lastDrop = units;
            maxY = units;
            extenderScale = extender.transform.localScale;
            StartCoroutine(ScaleDown());
        }
    }

    IEnumerator ScaleDown(){
        while(extender.transform.localScale.y < maxY){
            extender.transform.localScale += new Vector3(0, speed * Time.deltaTime, 0);
            extender.transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
            hand.transform.localPosition -= new Vector3(0, speed * Time.deltaTime * 2, 0);
            yield return null;
        }
        extender.transform.localScale = new Vector3(extender.transform.localScale.x, maxY, extender.transform.localScale.z);
    }
}
