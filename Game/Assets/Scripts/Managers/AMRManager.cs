using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AMRManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AMRManager AMRIM;
    public List<AMRController> AMRInstance = new List<AMRController>(new AMRController[10]);
    void Awake(){
        if(AMRIM == null)AMRIM = this;
        else Destroy(gameObject);
    }

    void Start(){
        Debug.Log($"AMR TOTAL: {AMRInstance.Count}");
    }

    public void AddAMR(int i, AMRController AMR){
        Debug.Log($"ADDING AMR AT INDEX: {i} {AMRInstance.Count}");
        AMRInstance[i] = AMR;
    }

    public AMRController GetAMR(int i){
        return AMRInstance[i];
    }
}
