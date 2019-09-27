using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile
{
    public List<BlockSave> world = new List<BlockSave>();

    public void Add(Vector3Int position, string name)
    {
        world.Add(new BlockSave(position, name));
    }
}

[Serializable]
public class BlockSave
{
    public int x;
    public int y;
    public int z;
    public string name;
    public string type;

    public BlockSave(Vector3Int position, string name)
    {
        x = position.x;
        y = position.y;
        z = position.z;

        this.name = name;
    }

    public BlockSave(Block block, string type = null)
    {
        this.x = block.Position().x;
        this.y = block.Position().y;
        this.z = block.Position().z;
        this.name = block.gameObject.name;
        this.type = type;
    }
}

