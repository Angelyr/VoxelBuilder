using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Slot : MonoBehaviour
{
    //Block
    private Button btn;
    private Inventory inventory;
    private Image image;
    private Color color;
    private Block block;

    private void Awake()
    {
        btn = GetComponent<Button>();
        inventory = transform.parent.parent.GetComponent<Inventory>();
        image = transform.Find("Image").GetComponent<Image>();
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

    public void Add(Block block)
    {
        this.block = block;
        transform.Find("Image").GetComponent<Image>().enabled = true;
        transform.Find("Image").GetComponent<Image>().sprite = ConvertToSprite(AssetPreview.GetAssetPreview(block.gameObject));
    }

    private static Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }


}
