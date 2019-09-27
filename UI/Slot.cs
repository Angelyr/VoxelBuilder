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

    //Monobehavior

    private void Awake()
    {
        btn = GetComponent<Button>();
        inventory = transform.parent.GetComponent<UI>();
        image = GetComponent<Image>();
        color = image.color;
        btn.onClick.AddListener(OnClick);
    }
    
    //Private

    private void ThumbnailProperties()
    {
        if (setThumbnailProperties) return;
        setThumbnailProperties = true;
        RuntimePreviewGenerator.BackgroundColor = new Color(1, 1, 1, 0);
        RuntimePreviewGenerator.OrthographicMode = true;
        RuntimePreviewGenerator.PreviewDirection = new Vector3(-1,-.5f,-.5f);
    }

    private void OnClick()
    {
        inventory.Select(this);
        image.color = Color.yellow;
        References.player.Select(block);
    }

    private static Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    //Public

    public void Select()
    {
        image.color = Color.yellow;
        References.player.Select(block);
    }

    public void DeSelect()
    {
        image.color = color;
    }

    public void Add(Block block)
    {
        if (block == null) return;
        this.block = block;
        transform.Find("Image").GetComponent<Image>().enabled = true;
        string name = block.TextureName();
        Texture2D texture = Resources.Load<Texture2D>("Sprites/" + name);
        transform.Find("Image").GetComponent<Image>().sprite = ConvertToSprite(texture);
    }

    public Block Get()
    {
        return block;
    }
}
