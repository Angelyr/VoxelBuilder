using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
}
