using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : UI
{
    List<Slot> slots;
    Queue<Block> blocks;
    int size;

    protected override void Awake()
    {
        base.Awake();
        slots = new List<Slot>();
        blocks = new Queue<Block>();
        size = transform.childCount;
    }

    //Public

    public void Add(Block block)
    {
        if (blocks.Contains(block)) return;

        if (blocks.Count >= size) blocks.Dequeue();
        blocks.Enqueue(block);
    }
}
