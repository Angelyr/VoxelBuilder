using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    protected Button button;

    protected virtual void Awake()
    {
        if (GetComponent<Button>())
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
    }

    protected virtual void OnClick() { }

    public virtual void Select(Slot target) { }
}
