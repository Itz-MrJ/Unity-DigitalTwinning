using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public int ID;
    private bool scheduleDrop = false;
    Collider other;

    // void Awake()
    // {
    // }

    void Start()
    {
        RobotInstance.RIM.AddSphere(ID, this);
    }

    public void Release(int id){
        if(id != ID)return;
        Debug.Log("Released!");
        scheduleDrop = true;
        other.transform.parent = null;
        other.GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(DelayedFunctionCoroutine(3f));
    }

    IEnumerator DelayedFunctionCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        disableRelease();
    }
    
    private void disableRelease(){
        scheduleDrop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible") && !scheduleDrop)
        {
            Debug.Log($"Holding {other}");
            scheduleDrop = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.localPosition = new Vector3(other.transform.localPosition.x, 0.0f, other.transform.localPosition.z);
            other.transform.parent = this.transform;
            this.other = other;
        }
    }
}
