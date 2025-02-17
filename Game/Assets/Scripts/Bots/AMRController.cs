using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    }

    public void MoveForward(int id, float distance)
    {
        if(id != ID)return;
        if (currentDirection == 3 || currentDirection == 1) transformPos = transform.position.z;
        else if (currentDirection == 0 || currentDirection == 2) transformPos = transform.position.x;
        Debug.Log($"Current Direction is: {currentDirection}");
        StartCoroutine(ToForward(currentDirection == 1 || currentDirection == 2? 0 - distance : distance));
    }

    public void Rotate(int id, float distance)
    {
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
