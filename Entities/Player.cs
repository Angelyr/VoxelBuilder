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

    new private Camera camera;
    private View view;
    private Inventory inventory;
    private bool lockCamera;
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

        LockCamera();
    }

    private void Update()
    {
        if (lockCamera) view.Move(architectMode);
        Inputs();
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
        MouseDirection();
        Zoom();

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

    private void InputDirection(string key, Vector3 direction)
    {
        if (Input.GetKeyDown(key)) this.direction += direction;
        if (Input.GetKeyUp(key)) this.direction -= direction;
        if (!Input.anyKey) this.direction = Vector3.zero;
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0) direction += Vector3.forward;
        if (scroll < 0) direction += Vector3.back;
    }

    private void MouseDirection()
    {
        if (Input.GetMouseButtonDown(2))
        {
            prevMouse = camera.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 mouse = camera.ScreenToViewportPoint(Input.mousePosition);
            direction = (prevMouse - mouse) * 100;
        }
        if (Input.GetMouseButtonUp(2))
        {
            direction = Vector3.zero;
        }
    }

    private void Place()
    {
        if (!lockCamera) return;
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

        float speed = 0f;

        if (architectMode) speed = Settings.dragSpeed;
        else speed = Settings.moveSpeed;

        transform.position = Vector3.MoveTowards(transform.position, target, speed);
    }

    private Vector3Int TargetAir()
    {
        Vector3 position = transform.position;
        Vector3 prevPosition = transform.position;
        float dist = 0f;
        while (World.Empty(Vector3Int.RoundToInt(position)))
        {
            prevPosition = position;
            Vector3 target = position + Target(Vector3.forward);
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
            Vector3 target = position + Target(Vector3.forward);
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
        Cursor.visible = true;
        lockCamera = false;
    }

    private void LockCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lockCamera = true;
    }

    private Vector3 Target(Vector3 targetDirection)
    {
        Vector3 target = camera.transform.TransformDirection(targetDirection);


        if (architectMode)
        {
            target = camera.ScreenPointToRay(Input.mousePosition).direction;
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

        Cursor.visible = architectMode;
    }
}
