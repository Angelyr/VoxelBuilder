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

    public string TextureName()
    {
        return transform.Find("Front").GetComponent<MeshRenderer>().sharedMaterial.name;
    }

    public Vector3Int Position()
    {
        return Vector3Int.RoundToInt(transform.position);
    }

    public Block Use(Vector3Int position)
    {
        if (!World.Empty(position)) return null;
        GameObject block = Instantiate(gameObject, position, Quaternion.identity, References.worldContainer.transform);
        block.name = gameObject.name;
        World.Add(position, block.GetComponent<Block>());
        return block.GetComponent<Block>();
    }

    

}
