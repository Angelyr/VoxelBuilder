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

    public void Create(Vector3 position)
    {
        Instantiate(gameObject, position, Quaternion.identity);
    }
}
