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
    private Inventory inventory;
    private bool lockedCamera;
    private bool airTargetMode = true;
    private bool extendMode = true;
    private bool architectMode = false;
    private Vector3Int lastDirection;
    
    //MonoBehavior

    private void Awake()
    {
        view = transform.Find("Camera").GetComponent<View>();
        camera = transform.Find("Camera").GetComponent<Camera>();
        inventory = transform.Find("Canvas/Tools/InventoryBtn").GetComponent<Inventory>();

        LockCamera();
    }

    private void Update()
    {
        if (lockedCamera) view.Move(architectMode);
        Inputs();
    }


    //Private 

    private void Inputs()
    {
        if (Input.GetKey("w")) Move(Vector3.forward);
        else if (Input.GetKey("s")) Move(Vector3.back);
        else if (Input.GetKey("a")) Move(Vector3.left);
        else if (Input.GetKey("d")) Move(Vector3.right);
        else if (Input.GetKey("left shift")) Move(Vector3.down);
        else if (Input.GetKey("space")) Move(Vector3.up);
        else Move(Vector3.zero);


        if (Input.GetMouseButtonDown(0)) Place();
        if (Input.GetMouseButtonDown(1)) Delete();
        if (Input.GetKeyDown("i"))
        {
            inventory.Toggle();
            if (inventory.Active()) UnlockCamera();
            else LockCamera();
        }
        if (Input.GetKeyDown("z")) ToggleAirTarget();
        if (Input.GetKeyDown("x")) ToggleExtend();
        if (Input.GetKeyDown("c")) ToggleArchitect();
    }

    private void Place()
    {
        if (!lockedCamera) return;
        Vector3Int target = TargetAir();

        if (target == Vector3Int.RoundToInt(Direction(Vector3.forward, 2)) && !airTargetMode) return;
        inventory.UseSelected(TargetAir());
        
        if (!World.Empty(target) && extendMode) World.Get(target).Extend(lastDirection);
    }

    private void Delete()
    {
        Vector3Int target = TargetBlock();
        if(!World.Empty(target) && extendMode)
        {
            World.Get(target).Subtract(lastDirection, null);
        }
        else World.Remove(TargetBlock());
    }

    private void Move(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        Vector3 target = transform.position + camera.transform.TransformDirection(direction);

        if (direction == Vector3.up || direction == Vector3.down)
        {
            target = transform.position + direction;
        }


        transform.position = Vector3.MoveTowards(transform.position, target, Settings.moveSpeed);
    }

    private Vector3Int TargetAir()
    {
        Vector3 position = transform.position;
        Vector3 prevPosition = transform.position;
        float dist = 0f;
        while (World.Empty(Vector3Int.RoundToInt(position)))
        {
            prevPosition = position;
            Vector3 target = position + camera.transform.TransformDirection(Vector3.forward);
            position = Vector3.MoveTowards(position, target, .1f);
            dist += .1f;

      
            if (dist >= Settings.range) return Vector3Int.RoundToInt(Direction(Vector3.forward, 2));
        }

        lastDirection = Vector3Int.RoundToInt(position) - Vector3Int.RoundToInt(prevPosition);
        return Vector3Int.RoundToInt(prevPosition);
    }

    private Vector3Int TargetBlock()
    {
        Vector3 position = transform.position;
        Vector3 prevPosition = transform.position;
        float dist = 0f;
        while (World.Empty(Vector3Int.RoundToInt(position)))
        {
            prevPosition = position;
            Vector3 target = position + camera.transform.TransformDirection(Vector3.forward);
            position = Vector3.MoveTowards(position, target, .1f);
            dist += .1f;
            if (dist >= Settings.range) return Vector3Int.RoundToInt(Direction(Vector3.forward, 2));
        }

        lastDirection = Vector3Int.RoundToInt(position) - Vector3Int.RoundToInt(prevPosition);
        return Vector3Int.RoundToInt(position);
    }

    private Vector3 Direction(Vector3 direction, float dist)
    {
        Vector3 target = transform.position + camera.transform.TransformDirection(direction*dist);
        return Vector3.MoveTowards(transform.position, target, dist);
    }

    private void UnlockCamera()
    {
        Cursor.lockState = CursorLockMode.None;
        lockedCamera = false;
    }

    private void LockCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lockedCamera = true;
    }

    //Public

    public void ToggleAirTarget()
    {
        airTargetMode = !airTargetMode;
    }

    public void ToggleExtend()
    {
        extendMode = !extendMode;
    }

    public void ToggleArchitect()
    {
        architectMode = !architectMode;
        if (architectMode) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    }
}
