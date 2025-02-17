using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider other;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible") && other.transform.parent == null)
        {
            // Debug.Log($"Holding {other}");
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.parent = this.transform;
            this.other = other;
        }
    }
}
