using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BeltDetector : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider other;
    private List<bool> TriggerList = new List<bool>() {false, false, false, false, false, false, false, false, false, false};
    void Start()
    {
        
    }
    private async Task WaitForSecondsAsync(float seconds)
    {
        await Task.Delay((int)(seconds * 1000)); // Convert seconds to milliseconds
    }
    private void ResetXZRotation(GameObject go){
        go.transform.rotation = Quaternion.Euler(0, go.transform.eulerAngles.y , 0);
    }

    private async Task OnTriggerEnter(Collider other)
    {
        Data od = other.GetComponent<Data>();
        Debug.Log($"NEW OBJECT PLACED ON BELT: {other} {TriggerList[0]} {TriggerList[1]} {od.ObjectID}");
        // if(TriggerList[od.ObjectID])return;
        // else TriggerList[od.ObjectID] = true;
        if (other.gameObject.CompareTag("Collectible") && other.transform.parent == null)
        {
            Debug.Log($"Holding {other}");
            ResetXZRotation(other.gameObject);
            await WaitForSecondsAsync(1);
            other.transform.parent = this.transform;
            this.other = other;

            StopperManager.SIM.GetSD(od.RobotID - 1).MoveIndefinitely(od.RobotID - 1, other, od.ObjectID, od.RobotID);
            // other.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
