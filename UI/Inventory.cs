using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class Inventory : UI
{
    //Blocks
    //Loaded from file

    private Slot selected;
   
    //Monobehavior

    protected override void Awake()
    {
        Load();
    }

    //Private

    private void Load()
    {
        GameObject[] blocks = Resources.LoadAll<GameObject>("Blocks");
        for(int i=0; i<blocks.Length; i++)
        {
            transform.GetChild(i).GetComponent<Slot>().Add(blocks[i].GetComponent<Block>());
        }

    }

    //Public

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public bool Active()
    {
        return gameObject.activeSelf;
    }

    public override void Select(Slot selected)
    {
        if (this.selected != null) this.selected.DeSelect();
        this.selected = selected;
    }    
}
