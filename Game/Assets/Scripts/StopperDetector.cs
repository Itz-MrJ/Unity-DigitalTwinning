using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperDetector : MonoBehaviour
{
    private Collider other;
    private float speed = 0.1f;
    private List<bool> ToStop = new List<bool>() {false, false, false, false, false, false, false, false, false, false};
    public int RobotID, ID;
    void Start()
    {
        StopperManager.SIM.AddSD(ID, this);
    }

    public void MoveIndefinitely(int StopperID, Collider other, int ObjectID, int RobotID){
        if(ID != StopperID) return;
        Debug.Log($"ABOUT TO MOVE COLLECTIBLE ID: {ObjectID} till {RobotID}");
        StartCoroutine(ToForward(other, RobotID));
        // Data od = other.gameObject.AddComponent<Data>();
        // od.RobotID = RobotID;
    }
    IEnumerator ToForward(Collider other, int RobotID)
    {
        float distance = 15.5f;
        Debug.Log($"{other.gameObject.transform.position.z}, {distance}, {ToStop[RobotID]}");
        while (other.gameObject.transform.position.z < distance && !ToStop[RobotID]){
            other.gameObject.transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.transform.parent}");
        Data od = other.GetComponent<Data>();
        Debug.Log($"STOPPER DETECTED &&: TargetRobotID: {od.RobotID} && RID: {ID}");
        if (other.gameObject.CompareTag("Collectible") && od.RobotID == RobotID)
        {
            Debug.Log($"DETECTED TO STOP BY {ID} & {other} & RobotID: {od.RobotID}");
            ToStop[od.RobotID] = true;
            other.transform.parent = null;
        }
    }
}
