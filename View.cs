using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    //Rotate
    //Pan
    //Zoom

    Camera cam;
    private float yaw = 0f;
    private float pitch = 0f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    
    public void FollowMouse()
    {
        yaw += Settings.cameraSpeed * Input.GetAxis("Mouse X");
        pitch -= Settings.cameraSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
