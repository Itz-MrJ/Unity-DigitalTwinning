using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static StopperManager SIM;
    public List<StopperDetector> SDInstance = new List<StopperDetector>(new StopperDetector[10]);
    void Awake(){
        if(SIM == null) SIM = this;
        else Destroy(gameObject);
    }

    public void AddSD(int i, StopperDetector SD){
        SDInstance[i] = SD;
    }

    public StopperDetector GetSD(int i){
        return SDInstance[i];
    }
}
