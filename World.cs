using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class World : MonoBehaviour
{
    //Blocks
    //Shared between players
    //Can be saved
    //Can be imported
    //Only one world
    //Players
    
    private static Dictionary<Vector3Int, Block> world;

    //Monobehavior

    private void Awake()
    {
        world = new Dictionary<Vector3Int, Block>();
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    //Private

    private void Save()
    {
        SaveFile save = new SaveFile();

        foreach(KeyValuePair<Vector3Int, Block> block in world)
        {
            save.Add(block.Key);
        }
        string json = JsonUtility.ToJson(save);
        string path = Application.persistentDataPath + "Save.json";
        File.WriteAllText(path, json);
    }

    private void Load()
    {
        string path = Application.persistentDataPath + "Save.json";
        string json = File.ReadAllText(path);
        Debug.Log(json);
    }

    //Public

    public static bool Add(Vector3Int position, Block target)
    {
        if (world.ContainsKey(position)) return false;
        world[position] = target;
        return true;
    }

    public static Block Get(Vector3Int position)
    {
        if (!world.ContainsKey(position)) return null;
        return world[position];
    }

    public static List<Vector3Int> GetSurrounding(Vector3Int position)
    {
        List<Vector3Int> area = new List<Vector3Int>();

        for(int x=-1; x<=1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    area.Add(position + new Vector3Int(x, y, z));
                }
            }
        }
        return area;
    }

    public static void Remove(Vector3Int position)
    {
        if (!world.ContainsKey(position)) return;
        Destroy(world[position].gameObject);
        world.Remove(position);
    }

    public static bool Empty(Vector3Int position)
    {
        return !world.ContainsKey(position);
    }

    

}
