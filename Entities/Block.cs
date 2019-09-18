using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Block : MonoBehaviour
{
    //Texture
    //Position
    //Placed
    //Inventory
    //Loaded from file

    protected MeshRenderer top;
    protected MeshRenderer bottom;
    protected MeshRenderer left;
    protected MeshRenderer right;
    protected MeshRenderer front;
    protected MeshRenderer back;

    //MonoBehavior

    protected virtual void Awake()
    {
        top = transform.Find("Top").GetComponent<MeshRenderer>();
        bottom = transform.Find("Bottom").GetComponent<MeshRenderer>();
        left = transform.Find("Left").GetComponent<MeshRenderer>();
        right = transform.Find("Right").GetComponent<MeshRenderer>();
        front = transform.Find("Front").GetComponent<MeshRenderer>();
        back = transform.Find("Back").GetComponent<MeshRenderer>();
    }


    //Public

    public Vector3Int Position()
    {
        return Vector3Int.RoundToInt(transform.position);
    }

    public Block Use(Vector3Int position)
    {
        if (!World.Empty(position)) return null;
        GameObject block = Instantiate(gameObject, position, Quaternion.identity, References.worldContainer.transform);
        World.Add(position, block.GetComponent<Block>());
        return block.GetComponent<Block>();
    }

    public void Extend(Vector3Int direction)
    {
        List<Vector3Int> surrounding = World.GetSurrounding(Position());

        foreach(Vector3Int target in surrounding)
        {
            if (direction.x != 0 && target.x != Position().x) continue;
            if (direction.y != 0 && target.y != Position().y) continue;
            if (direction.z != 0 && target.z != Position().z) continue;

            if (!World.Empty(target)) continue;
            if (World.Empty(target + direction)) continue;

            Block block = Use(target);
            block.Extend(direction);
        }
    }

    public void Subtract(Vector3Int direction, Block delete)
    {
        if(delete) World.Remove(delete.Position());
        List<Vector3Int> surrounding = World.GetSurrounding(Position());

        foreach (Vector3Int target in surrounding)
        {
            if (direction.x != 0 && target.x != Position().x) continue;
            if (direction.y != 0 && target.y != Position().y) continue;
            if (direction.z != 0 && target.z != Position().z) continue;

            if (World.Empty(target)) continue;
            World.Get(target).Subtract(direction, this);
        }
        World.Remove(Position());
    }

}
