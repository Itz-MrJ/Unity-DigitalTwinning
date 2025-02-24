using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockerManager : MonoBehaviour
{
    public static DockerManager DMIM;
    private List<bool> DMInstance = new List<bool>() {false, false, false, false, false, false, false, false, false, false};
    void Awake()
    {
        if(DMIM == null)DMIM = this;
        else Destroy(gameObject);
    }

    public void SetDocker(int i, bool activation){
        DMInstance[i] = activation;
    }

    public bool GetDocker(int i){
        return DMInstance[i];
    }

    public int GetLength(){
        Debug.Log("IN DMIM");
        return DMInstance.Count;
    }
}
