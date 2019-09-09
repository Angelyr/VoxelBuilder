using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    //Blocks
    //Shared between players
    //Can be saved
    //Can be imported
    //Only one world
    //Players


    private static Dictionary<Vector3Int, GameObject> world;

    private void Awake()
    {
        world = new Dictionary<Vector3Int, GameObject>();
    }

    public static bool Add(Vector3Int position, GameObject target)
    {
        if (world.ContainsKey(position)) return false;
        world[position] = target;
        return true;
    }

    public static bool Empty(Vector3Int position)
    {
        return !world.ContainsKey(position);
    }
}
