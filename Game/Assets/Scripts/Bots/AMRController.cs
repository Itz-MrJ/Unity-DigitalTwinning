using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class AMRController : MonoBehaviour
{
    private Quaternion ARMPosition;
    private float speed = 1f, transformPos;
    private AMRPositions coords = new AMRPositions(0, 1, 2, 3);
    private int currentDirection = 3;
    public int ID;
   void Start()
    {
        AMRManager.AMRIM.AddAMR(ID, this);
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<NavMeshObstacle>().enabled = true;
    }

    void SetGridPath(Vector3 destination, Queue<Vector3> waypoints, NavMeshAgent agent, int id){
        waypoints.Clear();
        NavMeshPath navPath = new NavMeshPath();

        if (agent.CalculatePath(destination, navPath) && navPath.corners.Length > 1)
        {
            Debug.Log($"NAVPATH: {navPath.corners.Length}");
            for (int i = 0; i < navPath.corners.Length; i++)
            {
                Debug.Log($"INDEX: {i}\n NAVPATH: {navPath.corners[i]}");
            }
            List<Vector3> adjustedPath = ConvertToGridPath(navPath.corners);
            foreach (Vector3 point in adjustedPath)
            {
                waypoints.Enqueue(point);
            }
            AMRManager.AMRIM.SetQueue(id, waypoints);
        }
    }

    List<Vector3> ConvertToGridPath(Vector3[] originalPath)
    {
        List<Vector3> gridPath = new List<Vector3>();
        Vector3 lastPoint = originalPath[0];

        for (int i = 1; i < originalPath.Length; i++)
        {
            Vector3 nextPoint = originalPath[i];

            // Move along X first
            while (Mathf.Round(lastPoint.x) != Mathf.Round(nextPoint.x))
            {
                lastPoint.x += Mathf.Sign(nextPoint.x - lastPoint.x);
                gridPath.Add(new Vector3(lastPoint.x, lastPoint.y, lastPoint.z));
            }

            // Move along Z next
            while (Mathf.Round(lastPoint.z) != Mathf.Round(nextPoint.z))
            {
                lastPoint.z += Mathf.Sign(nextPoint.z - lastPoint.z);
                gridPath.Add(new Vector3(lastPoint.x, lastPoint.y, lastPoint.z));
            }
        }
        return gridPath;
    }

    void MoveAlongPath(Queue<Vector3> waypoints, Transform t, NavMeshAgent agent, int id)
    {
        Debug.Log($"MOVING ALNG PATH: {AMRManager.AMRIM.GetQueue(id).Count}");
        if (AMRManager.AMRIM.GetQueue(id).Count == 0) return;

        Vector3 nextPoint = AMRManager.AMRIM.GetQueue(id).Peek();
        Debug.Log($"NEXT POINT: {nextPoint}");
        t.position = Vector3.MoveTowards(t.position, nextPoint, agent.speed * Time.deltaTime);

        Debug.Log($"MOVing agent in path {t.position} {Vector3.Distance(t.position, nextPoint) < 0.1f}");
        if (Vector3.Distance(t.position, nextPoint) < 0.1f)
        {
            AMRManager.AMRIM.GetQueue(id).Dequeue();
            RotateToFaceNextPoint(id, t);
        }
        // AMRManager.AMRIM.GetQueue(id).Clear();
        // AMRManager.AMRIM.SetQueue(id, waypoints);
    }
    void RotateToFaceNextPoint(int id, Transform t)
    {
        Debug.Log($"ROTATING TO FACE POint: {AMRManager.AMRIM.GetQueue(id).Count}");
        if (AMRManager.AMRIM.GetQueue(id).Count == 0) return;

        Vector3 direction = AMRManager.AMRIM.GetQueue(id).Peek() - t.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        t.rotation = Quaternion.Euler(0, angle, 0);
    }

    private async Task WaitForSecondsAsync(float seconds)
    {
        await Task.Delay((int)(seconds * 50)); // Convert seconds to milliseconds
    }
    async Task Update(){
        int index = AMRManager.AMRIM.GetMoving(ID);
        if(index >= 0){
            GameObject ob = AMRManager.AMRIM.GetAMR(ID).gameObject;
            // Debug.Log($"INSINDEHSHSEAF {AMRManager.AMRIM.GetLastPosition(ID).HasValue} {AMRManager.AMRIM.GetLastPosition(ID).Value} {ob.transform.position} {Vector3.Distance(AMRManager.AMRIM.GetLastPosition(ID).Value, ob.transform.position) == 0f} {Vector3.Distance(AMRManager.AMRIM.GetDestination(ID).Value, ob.transform.position) <= 0.1f}");
            if(AMRManager.AMRIM.GetLastPosition(ID).HasValue && Vector3.Distance(AMRManager.AMRIM.GetLastPosition(ID).Value, ob.transform.position) == 0f && Vector3.Distance(AMRManager.AMRIM.GetDestination(ID).Value, ob.transform.position) <= 0.1f){
                AMRManager.AMRIM.SetMoving(ID, -1);
                ob.GetComponent<NavMeshAgent>().enabled = false;
                await WaitForSecondsAsync(1);
                ob.GetComponent<NavMeshObstacle>().enabled = true;
                AMRManager.AMRIM.SetLastPosition(ID, null);
            }else{
                Vector3 pos1 = AMRManager.AMRIM.GetLastPosition(ID).Value;
                Vector3 pos2 = ob.transform.position;
                Vector3 roundedPos1 = new Vector3(
                    Mathf.Round(pos1.x * 100f) / 100f,
                    Mathf.Round(pos1.y * 100f) / 100f,
                    Mathf.Round(pos1.z * 100f) / 100f
                );
                Vector3 roundedPos2 = new Vector3(
                    Mathf.Round(pos2.x * 100f) / 100f,
                    Mathf.Round(pos2.y * 100f) / 100f,
                    Mathf.Round(pos2.z * 100f) / 100f
                );
                // Debug.Log($"COMPARING {roundedPos1}, {roundedPos2} {roundedPos1 == roundedPos2}");
                AMRManager.AMRIM.SetLastPosition(ID, ob.transform.position);
                if(roundedPos1 != roundedPos2) RobotInstance.RIM.SendCommand($"AMR: {ID}\nPosition: {pos2}", "listener");
            }
            // For Grid Movement -
            // GameObject ob = AMRManager.AMRIM.GetAMR(ID).gameObject;
            // Vector3[] vecs = AMRManager.AMRIM.GetQueue(ID).ToArray();
            // if(index >= vecs.Length) {
            //     ob.GetComponent<NavMeshAgent>().enabled = false;
            //     await WaitForSecondsAsync(1);
            //     ob.GetComponent<NavMeshObstacle>().enabled = true;
            //     AMRManager.AMRIM.SetMoving(ID, -1);
            //     return;
            // }
            // if(Vector3.Distance(vecs[index], ob.transform.position) < 0.1f) AMRManager.AMRIM.SetMoving(ID, -99);
            // else ob.GetComponent<NavMeshAgent>().SetDestination(vecs[index]);
        }
        if (Input.GetMouseButtonDown(0)){
            // Check if UI was clicked
            if (EventSystem.current.IsPointerOverGameObject()) return;
            string temp = ButtonFunctionality.BFM.GetBot();
            if(temp != null){
                string[] part = temp.Split(' ');
                if(int.Parse(part[1]) == ID && part[0] == "AMR"){
                    Ray ray = AMRManager.AMRIM.camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if(Physics.Raycast(ray, out hit)){
                        string[] position = hit.collider.gameObject.name.Split(' ');
                        GameObject AMR = AMRManager.AMRIM.GetAMR(ID).gameObject;
                        AMR.GetComponent<NavMeshObstacle>().enabled = false;
                        await WaitForSecondsAsync(1);
                        NavMeshAgent agent = AMR.GetComponent<NavMeshAgent>();
                        agent.enabled = true;
                        // Not docker
                        if(position.Length != 3){
                            NavMeshPath navPath = new NavMeshPath();
                            agent.CalculatePath(hit.point, navPath);
                            if(navPath.corners.Length == 0)return;
                            Data od = AMR.GetComponent<Data>();
                            if (od != null) {
                                DockerManager.DMIM.SetDocker(od.RobotID, false);
                                Destroy(RobotInstance.RIM.GetBody(od.RobotID).gameObject.GetComponent<Data>());
                                Destroy(AMR.GetComponent<Data>());
                            }
                            AMRManager.AMRIM.SetDestination(ID, hit.point);
                            AMRManager.AMRIM.SetLastPosition(ID, AMR.transform.position);
                            AMRManager.AMRIM.SetMoving(ID, 0);
                            Debug.Log($"ABOUT TO MOVEEEEE {hit.point} {navPath.corners.Length}");
                            agent.SetDestination(hit.point);
                            // SetGridPath(hit.point, AMRManager.AMRIM.GetQueue(ID), agent, ID);
                            // Debug.Log($"QUEUE AFTER MAKING GRID OR SOMETHING: {AMRManager.AMRIM.GetQueue(ID).Count} && {AMR.transform.position}");
                        }
                        else{
                            Debug.Log($"{position[0]} && {position[1]} && {position[2]}");
                            Debug.Log(DockerManager.DMIM.GetLength());
                            // Check if there already is load at docker
                            if(DockerManager.DMIM.GetDocker(int.Parse(position[1]))) return;
                            else DockerManager.DMIM.SetDocker(int.Parse(position[1]), true);
                            GameObject docker = GameObject.Find(hit.collider.gameObject.name);
                            // Set it's Robot Docker ID to the AMR;
                            Data od = AMR.AddComponent<Data>();
                            // Add AMR ID to check for Collectible
                            Data rod = RobotInstance.RIM.GetBody(int.Parse(position[1])).gameObject.AddComponent<Data>();
                            rod.RobotID = ID;
                            od.RobotID = int.Parse(position[1]);
                            od.ObjectID = ID;
                            agent.SetDestination(docker.transform.position);
                        }
                    }
                }
            }

        }
    }

    public void MoveForward(int id, float distance){
        if(id != ID || distance < 0.0f)return;
        if (currentDirection == 3 || currentDirection == 1) transformPos = transform.position.z;
        else if (currentDirection == 0 || currentDirection == 2) transformPos = transform.position.x;
        Debug.Log($"Current Direction is: {currentDirection}");
        StartCoroutine(ToForward(currentDirection == 1 || currentDirection == 2? 0 - distance : distance));
    }

    public void Rotate(int id, float distance)
    {
        Debug.Log($"ROTATING AMR NOW {id}");
        if(id != ID)return;
        if(90f != Math.Abs(distance))return;
        int finalPos = (int) (transform.eulerAngles.y + distance);
        finalPos = finalPos < 0 ? 270 : finalPos;
        int abs = (int) Math.Abs(finalPos) / 90;
        currentDirection = abs == 4 ? 0 : abs;
        ARMPosition = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + distance, transform.eulerAngles.z);
        StartCoroutine(RotateBot(currentDirection));
    }

    IEnumerator RotateBot(float abs)
    {
        while (Quaternion.Angle(transform.rotation, ARMPosition) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ARMPosition, 20f * Time.deltaTime);
            yield return null;
        }
        RobotInstance.RIM.SendCommand("end_time", "client");
        if (abs == 0) transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        transform.rotation = ARMPosition;
    }

    IEnumerator ToForward(float distance)
    {
        float finalPos = distance + transformPos;
        Debug.Log($"{finalPos} {distance} {transformPos}");
        if (distance > 0)
            while (transformPos < finalPos)
            {
                // Going North / 0
                if (currentDirection == 3)
                {
                    transform.position += new Vector3(0, 0, speed * Time.deltaTime);
                    transformPos = transform.position.z;
                }
                // Going East / 90
                else if (currentDirection == 0)
                {
                    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                    transformPos = transform.position.x;
                }
                yield return null;
            }
        else
            while (transformPos > finalPos)
            {
                // Going South / 180
                if (currentDirection == 1)
                {
                    transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
                    transformPos = transform.position.z;
                }
                else if (currentDirection == 2)
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                    transformPos = transform.position.x;
                }
                yield return null;
            }
        RobotInstance.RIM.SendCommand("end_time", "client");
        if (currentDirection == 3 || currentDirection == 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, finalPos);
            transformPos = transform.position.z;
        }
        else if (currentDirection == 0 || currentDirection == 2)
        {
            transform.position = new Vector3(finalPos, transform.position.y, transform.position.z);
            transformPos = transform.position.x;
        }
    }
}
