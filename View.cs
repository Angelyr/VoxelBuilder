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
    private Vector3Int target = Vector3Int.zero;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Move(bool architect)
    {
        if (architect) Rotate();
        else FollowMouse();
    }
    
    private void FollowMouse()
    {
        yaw += Settings.cameraSpeed * Input.GetAxis("Mouse X");
        pitch -= Settings.cameraSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    private void Rotate()
    {
        //transform.Translate(Vector3.right * Time.deltaTime);
        transform.LookAt(target);
    }
}
