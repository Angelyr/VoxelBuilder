using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : UI
{
    List<Slot> slots;
    Queue<Block> blocks;
    Slot selected;
    int size;

    protected override void Awake()
    {
        base.Awake();
        slots = new List<Slot>();
        foreach (Transform child in transform) slots.Add(child.GetComponent<Slot>());
        blocks = new Queue<Block>();
        size = transform.childCount;
    }

    //Public

    public void Add(Block block)
    {
        if (blocks.Contains(block)) return;

        if (blocks.Count >= size) blocks.Dequeue();
        blocks.Enqueue(block);

        for(int i=size-1; i>0; i--)
        {
            slots[i].Add(slots[i - 1].Get());
        }
        slots[0].Add(block);
    }

    public void Select(int target)
    {
        if (selected != null) selected.DeSelect();
        selected = slots[target];
        slots[target].Select();
    }
}
