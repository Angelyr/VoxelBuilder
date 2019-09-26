using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Slot : MonoBehaviour
{
    //Block
    private Button btn;
    private UI inventory;
    private Image image;
    private Color color;
    private Block block;
    private bool setThumbnailProperties = false;

    private void Awake()
    {
        btn = GetComponent<Button>();
        inventory = transform.parent.GetComponent<UI>();
        image = GetComponent<Image>();
        color = image.color;
        btn.onClick.AddListener(Select);
    }

    private void ThumbnailProperties()
    {
        if (setThumbnailProperties) return;
        setThumbnailProperties = true;
        RuntimePreviewGenerator.BackgroundColor = new Color(1, 1, 1, 0);
        RuntimePreviewGenerator.OrthographicMode = true;
        RuntimePreviewGenerator.PreviewDirection = new Vector3(-1,-.5f,-.5f);
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
        string name = block.TextureName();
        Texture2D texture = Resources.Load<Texture2D>("Sprites/" + name);
        transform.Find("Image").GetComponent<Image>().sprite = ConvertToSprite(texture);
    }

    public void Use(Vector3Int target)
    {
        block.Use(target);
    }

    private static Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }


}
