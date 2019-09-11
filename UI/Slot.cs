using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //Block
    private Button btn;
    private Inventory inventory;
    private Image image;
    private Color color;

    private void Awake()
    {
        btn = GetComponent<Button>();
        inventory = transform.parent.parent.GetComponent<Inventory>();
        image = GetComponent<Image>();
        color = image.color;
        btn.onClick.AddListener(Select);
    }

    private void Select()
    {
        inventory.Select(this);
        image.color = Color.yellow;
    }

    //Public

    public void DeSelect()
    {
        image.color = color;
    }


}
