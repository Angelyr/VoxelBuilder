using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //Texture
    //Position
    //Placed
    //Inventory
    //Loaded from file

    public void Create(Vector3Int position)
    {
        if (!World.Empty(position)) return;
        GameObject block = Instantiate(gameObject, position, Quaternion.identity);
        World.Add(position, block);
    }
}
