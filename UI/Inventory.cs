﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class Inventory : MonoBehaviour
{
    //Blocks
    //Loaded from file

    private GameObject inventoryUI;
    private Button invetoryBtn;
    private Slot selected;
   
    //Monobehavior

    private void Awake()
    {
        inventoryUI = transform.Find("Inventory").gameObject;
        invetoryBtn = GetComponent<Button>();
        invetoryBtn.onClick.AddListener(Toggle);
        Load();
    }

    //Private

    private void Load()
    {
        GameObject[] blocks = Resources.LoadAll<GameObject>("Blocks");
        for(int i=0; i<blocks.Length; i++)
        {
            inventoryUI.transform.GetChild(i).GetComponent<Slot>().Add(blocks[i].GetComponent<Block>());
        }

    }

    //Public

    public void Toggle()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public bool Active()
    {
        return inventoryUI.activeSelf;
    }

    public void Select(Slot selected)
    {
        if (this.selected != null) this.selected.DeSelect();
        this.selected = selected;
    }

    public void UseSelected(Vector3Int target)
    {
        if (selected == null) return;
        selected.Use(target);
    }

    
}
