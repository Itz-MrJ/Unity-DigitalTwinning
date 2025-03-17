using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public static FreeCamera FCI;
    public float moveSpeed = 3f;  // Speed of movement
    public float lookSpeed = 1f;   // Sensitivity of mouse movement
    public float scrollSpeed = 1f; // Speed of zooming
    public bool UI = false;
    private float yaw = 0f;
    private float pitch = 0f;
    void Awake(){
        if (FCI) Destroy(gameObject);
        else FCI = this;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide cursor and lock it to the center
    }

    void Update()
    {
        if(UI)return;
        // Mouse Look
        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch -= lookSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limit up/down rotation

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // WASD Movement
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(moveX, 0, moveZ);

        // Up and Down Movement (QE keys)
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Scroll to zoom in and out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scroll * scrollSpeed);
        
        // Unlock cursor when pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
    }
}

