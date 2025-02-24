using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class AMRManager : MonoBehaviour
{
    public static AMRManager AMRIM;
    public Camera camera;
    private List<AMRController> AMRInstance = new List<AMRController>(new AMRController[10]);
    private List<Vector3?> LastPosition = new List<Vector3?>();
    private List<Vector3?> Destination = new List<Vector3?>();
    private List<Queue<Vector3>> WayPoints = new List<Queue<Vector3>>();
    private List<int> IsMoving = new List<int>();
    private List<bool> IsMovingBool = new List<bool>();
    void Awake(){
        if(AMRIM == null)AMRIM = this;
        else Destroy(gameObject);
    }
    void Start(){
        for (int i = 0; i < 10; i++)
        {
            WayPoints.Add(new Queue<Vector3>());
            IsMoving.Add(-1);
            IsMovingBool.Add(false);
            LastPosition.Add(null);
            Destination.Add(null);
        }
        // GameObject[] objects = GameObject.FindGameObjectsWithTag("Collectible");
        // foreach (GameObject item in objects){
        //     item.SetActive(false);
        // }
        // surface.GetComponent<NavMeshSurface>().BuildNavMesh();
        // foreach (GameObject item in objects){
        //     item.SetActive(true);
        // }
    }
    public void SetDestination(int i, Vector3? vec){
        if(vec.HasValue) Destination[i] = vec.Value;
        else Destination[i] = null;
    }
    public Vector3? GetDestination(int i){
        return Destination[i];
    }
    public Vector3? GetLastPosition(int i){
        return LastPosition[i];
    }
    public void SetLastPosition(int i, Vector3? vec){
        if(vec.HasValue) LastPosition[i] = vec.Value;
        
        else LastPosition[i] = null;
    }
    public int GetMoving(int i){
        return IsMoving[i];
    }
    public bool GetMovingBool(int i){
        return IsMovingBool[i];
    }
    public void SetMovingBool(int i, bool val){
        IsMovingBool[i] = val;
    }
    public void SetMoving(int i, int index){
        if(index == -99) {
            IsMoving[i]+=1;
            IsMovingBool[i] = true;
        }
        else IsMoving[i] = index;
    }
    public Queue<Vector3> GetQueue(int i){
        return WayPoints[i];
    }
    public void SetQueue(int i, Queue<Vector3> q){
        WayPoints[i] = q;
    }
    public void AddAMR(int i, AMRController AMR){
        AMRInstance[i] = AMR;
    }

    public AMRController GetAMR(int i){
        return AMRInstance[i];
    }
}
