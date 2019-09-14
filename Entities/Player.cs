﻿using System.Collections;
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
    private bool lockedCursor;
    private bool airTargetMode = true;
    private bool extendMode = true;
    private bool architectMode = false;
    private Vector3 direction = Vector3.zero;
    private Vector3Int lastDirection;
    private Vector3 prevMouse;
    
    //MonoBehavior

    private void Awake()
    {
        view = transform.Find("Camera").GetComponent<View>();
        camera = transform.Find("Camera").GetComponent<Camera>();
        inventory = transform.Find("Canvas/Tools/InventoryBtn").GetComponent<Inventory>();

        LockCursor();
    }

    private void Update()
    {
        if (lockedCursor) view.Move(architectMode);
        Inputs();
        //Rotate();
        Move(direction);
    }


    //Private 

    private void Inputs()
    {
        InputDirection("w", Vector3.forward);
        InputDirection("s", Vector3.back);
        InputDirection("a", Vector3.left);
        InputDirection("d", Vector3.right);
        InputDirection("space", Vector3.up);
        InputDirection("left shift", Vector3.down);

        if (Input.GetMouseButtonDown(0)) Place();
        if (Input.GetMouseButtonDown(1)) Delete();
        if (Input.GetKeyDown("i"))
        {
            inventory.Toggle();
            if (inventory.Active()) UnlockCursor();
            else LockCursor();
        }
        if (Input.GetKeyDown("z")) ToggleAirTarget();
        if (Input.GetKeyDown("x")) ToggleExtend();
        if (Input.GetKeyDown("c")) ToggleArchitect();

        
    }

    private void InputDirection(string key, Vector3 direction)
    {
        if (Input.GetKeyDown(key)) this.direction += direction;
        if (Input.GetKeyUp(key)) this.direction -= direction;
    }

    private void Place()
    {
        if (!lockedCursor) return;
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
        float up = direction.y;
        direction.y = 0;
        Vector3 target = transform.position + camera.transform.TransformDirection(direction);
        target.y = transform.position.y + up;
        


        transform.position = Vector3.MoveTowards(transform.position, target, Settings.moveSpeed);
    }

    private void Rotate()
    {
        if (Input.GetMouseButtonDown(2))
        {
           
        }
        if (Input.GetMouseButton(2))
        {
            Move(Vector3.right);
        }
    }

    private Vector3Int TargetAir()
    {
        Vector3 position = transform.position;
        Vector3 prevPosition = transform.position;
        float dist = 0f;
        while (World.Empty(Vector3Int.RoundToInt(position)))
        {
            prevPosition = position;
            Vector3 target = position + Target();
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
            Vector3 target = position + Target();
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

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        lockedCursor = false;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lockedCursor = true;
    }

    private Vector3 Target()
    {
        Vector3 target = camera.transform.TransformDirection(Vector3.forward);


        if (architectMode)
        {
            Ray direction = camera.ScreenPointToRay(Input.mousePosition);
            return direction.direction;
        }
        
        return target;
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
