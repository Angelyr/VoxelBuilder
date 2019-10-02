using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera cam;
    private View view;
    private Inventory inventory;
    private bool cameraMove;
    private bool airTargetMode = true;
    private bool extendMode = true;
    private bool architectMode = false;
    private bool replace = false;
    private Vector3 direction = Vector3.zero;
    private Vector3Int lastDirection;
    private Vector3 prevMouse;
    private Block selected;
    private Hotbar hotbar;
    private Stack<List<BlockSave>> history;
    private GameObject menu;
    private float buildTimer = 0;
    
    //MonoBehavior

    private void Awake()
    {
        view = transform.Find("Camera").GetComponent<View>();
        cam = transform.Find("Camera").GetComponent<Camera>();
        inventory = transform.Find("Canvas/Inventory").GetComponent<Inventory>();
        hotbar = transform.Find("Canvas/Hotbar").GetComponent<Hotbar>();
        history = new Stack<List<BlockSave>>();
        menu = transform.Find("Canvas/Menu").gameObject;

        CameraCanMove();
    }

    private void Update()
    {
        if (cameraMove) view.Move(architectMode);
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

        if (Input.GetMouseButton(0) && !replace) Place();
        if (Input.GetMouseButton(0) && replace) Replace();
        if (Input.GetMouseButton(1)) Delete();
        if (Input.GetMouseButtonDown(2)) GetBlock();
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) buildTimer = 0;
        if (Input.GetKeyDown("i")) ToggleInventory();
        if (Input.GetKeyDown("u")) Undo();
        if (Input.GetKeyDown("z")) ToggleAirTarget();
        if (Input.GetKeyDown("e")) ToggleExtend();
        if (Input.GetKeyDown("c")) ToggleArchitect();
        if (Input.GetKeyDown("r")) ToggleReplace();
        if (Input.GetKeyDown("escape")) ToggleMenu();
        for (int i = 1; i <= 5; i++) if (Input.GetKeyDown(i + "")) hotbar.Select(i-1); 

        
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
            prevMouse = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 mouse = cam.ScreenToViewportPoint(Input.mousePosition);
            direction = (prevMouse - mouse) * 100;
        }
        if (Input.GetMouseButtonUp(2))
        {
            direction = Vector3.zero;
        }
    }

    private bool Timer(float time)
    {
        float timer = buildTimer;
        buildTimer += Time.deltaTime;
        if (buildTimer >= time) buildTimer = 0;
        return timer == 0;
    }

    private void Place()
    {
        if (!cameraMove) return;
        if (selected == null) return;
        if (!Timer(.5f)) return;
        Vector3Int target = TargetAir();

        if (target == Vector3Int.RoundToInt(Direction(Vector3.forward, 2)) && !airTargetMode) return;

        Block block = selected.Use(target);
        if (block)
        {
            history.Push(new List<BlockSave>());
            history.Peek().Add(new BlockSave(block, "build"));
        }


        if (!World.Empty(target) && extendMode) ExtendBuild(lastDirection, target);
    }

    private void Delete()
    {
        if (!Timer(.5f)) return;
        Vector3Int target = TargetBlock();
        if (World.Empty(target)) return;

        history.Push(new List<BlockSave>());

        if (extendMode)
        {
            ExtendDelete(lastDirection, target);
        }
        else
        {
            history.Peek().Add(new BlockSave(World.Get(target), "delete"));
            World.Remove(target);
        }
    }

    private void Replace()
    {
        if (!Timer(.5f)) return;
        Vector3Int target = TargetBlock();
        if (World.Empty(target)) return;
        history.Push(new List<BlockSave>());

        if(extendMode)
        {
            ExtendReplace(lastDirection, target, new List<Vector3Int>());
        }
        else
        {
            history.Peek().Add(new BlockSave(World.Get(target), "replace"));
            World.Remove(target);
            selected.Use(target);
        }
    }

    private void Move(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        float up = direction.y;
        direction.y = 0;
        Vector3 target = transform.position + cam.transform.TransformDirection(direction);
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
        Vector3 target = transform.position + cam.transform.TransformDirection(direction*dist);
        return Vector3.MoveTowards(transform.position, target, dist);
    }

    

    private Vector3 Target(Vector3 targetDirection)
    {
        Vector3 target = cam.transform.TransformDirection(targetDirection);

        if (architectMode)
        {
            target = cam.ScreenPointToRay(Input.mousePosition).direction;
        }
        
        return target;
    }

    private void ExtendReplace(Vector3Int direction, Vector3Int position, List<Vector3Int> prev)
    {
        history.Peek().Add(new BlockSave(World.Get(position), "replace"));
        World.Remove(position);
        selected.Use(position);
        prev.Add(position);
        List<Vector3Int> surrounding = World.GetSurrounding(position);

        foreach (Vector3Int target in surrounding)
        {
            if (direction.x != 0 && target.x != position.x) continue;
            if (direction.y != 0 && target.y != position.y) continue;
            if (direction.z != 0 && target.z != position.z) continue;

            if (World.Empty(target)) continue;
            if (prev.Contains(target)) continue;
            ExtendReplace(direction, target, prev);
        }
    }

    private void ExtendBuild(Vector3Int direction, Vector3Int position)
    {
        List<Vector3Int> surrounding = World.GetSurrounding(position);

        foreach (Vector3Int target in surrounding)
        {
            if (direction.x != 0 && target.x != position.x) continue;
            if (direction.y != 0 && target.y != position.y) continue;
            if (direction.z != 0 && target.z != position.z) continue;

            if (!World.Empty(target)) continue;
            if (World.Empty(target + direction)) continue;

            Block block = selected.Use(target);
            if(block) history.Peek().Add(new BlockSave(block, "build"));
            ExtendBuild(direction, target);
        }
    }

    private void ExtendDelete(Vector3Int direction, Vector3Int position)
    {
        history.Peek().Add(new BlockSave(World.Get(position), "delete"));
        World.Remove(position);
        List<Vector3Int> surrounding = World.GetSurrounding(position);

        foreach (Vector3Int target in surrounding)
        {
            if (direction.x != 0 && target.x != position.x) continue;
            if (direction.y != 0 && target.y != position.y) continue;
            if (direction.z != 0 && target.z != position.z) continue;

            if (World.Empty(target)) continue;
            ExtendDelete(direction, target);
        }
    }

    

    private void CameraCantMove()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cameraMove = false;
    }

    private void CameraCanMove()
    {
        if (!architectMode) Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = architectMode;
        cameraMove = true;
    }

    private void GetBlock()
    {
        Block block = World.Get(TargetBlock());
        if (block == null) return;
        selected = Resources.Load<GameObject>("Blocks/" + block.name).GetComponent<Block>();
        hotbar.Add(selected);
        hotbar.Select(selected);
    }

    //Public

    public void ToggleReplace()
    {
        replace = !replace;
    }

    public void Undo()
    {
        if (history.Count == 0) return;
        foreach (BlockSave block in history.Pop())
        {
            if (block.type == "build")
            {
                World.Remove(new Vector3Int(block.x, block.y, block.z));
            }
            if (block.type == "delete")
            {
                World.Create(block);
            }
            if (block.type == "replace")
            {
                World.Remove(new Vector3Int(block.x, block.y, block.z));
                World.Create(block);
            }
        }
    }

    public void ToggleMenu()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            Cursor.visible = architectMode;
            if (!architectMode) Cursor.lockState = CursorLockMode.Locked;
            cameraMove = true;
        }
        else
        {
            menu.SetActive(true);
            Cursor.visible = true;
            cameraMove = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Select(Block block)
    {
        selected = block;
        hotbar.Add(block);
    }

    public void ToggleInventory()
    {
        inventory.Toggle();
        if (inventory.Active()) CameraCantMove();
        else CameraCanMove();
    }

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
