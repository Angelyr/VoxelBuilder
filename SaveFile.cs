using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile
{
    public List<Position> world = new List<Position>();

    public void Add(Vector3Int position)
    {
        world.Add(new Position(position));
    }
}

[Serializable]
public class Position
{
    public int x;
    public int y;
    public int z;
    //BlockSave block;

    public Position(Vector3Int position)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }
}

public class BlockSave
{
}
