using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Keep in mind multiple players
    //UI
    //Inventory
    //Hotbar
    //Options
    //In World
    //Move
    //Controls
    //Camera

    private new Camera camera;
    private View view;
    private Block block;
    
    //MonoBehavior

    private void Awake()
    {
        view = transform.Find("Camera").GetComponent<View>();
        camera = transform.Find("Camera").GetComponent<Camera>();
        block = Resources.Load<GameObject>("Block").GetComponent<Block>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        view.FollowMouse();
        Inputs();
    }


    //Private 

    private void Inputs()
    {
        if (Input.GetKey("w")) Move(Vector3.forward);
        else if (Input.GetKey("s")) Move(Vector3.back);
        else if (Input.GetKey("a")) Move(Vector3.left);
        else if (Input.GetKey("d")) Move(Vector3.right);
        else if (Input.GetKey("left shift")) Move(Vector3.up);
        else if (Input.GetKey("left ctrl")) Move(Vector3.down);
        else Move(Vector3.zero);


        if (Input.GetMouseButton(0)) Place();
        
    }

    private void Place()
    {
        //check if area is selected then place a block there else place one in front of you
        block.Create(Direction(Vector3.forward));

    }

    private void Move(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        Vector3 target = transform.position + camera.transform.TransformDirection(direction);
        transform.position = Vector3.MoveTowards(transform.position, target, Settings.moveSpeed);
    }

    private Vector3 Direction(Vector3 direction)
    {
        Vector3 target = transform.position + camera.transform.TransformDirection(direction);
        return Vector3.MoveTowards(transform.position, target, 2);
    }
}
