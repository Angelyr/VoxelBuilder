using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    private Button button;
    private Player player;

    private void Start()
    {
        player = References.player;
        if (GetComponent<Button>())
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
            OnClick();
            OnClick();
        }
        Transform control = transform.Find("Control");
        if (control)
        {
            control.GetComponent<TMP_InputField>().onValueChanged.AddListener(ChangeHotkey);
        }
    }

    private void ChangeHotkey(string input)
    {
        Debug.Log(input);
    }

    private void OnClick() {
        if (gameObject.name == "Continue") player.ToggleMenu("Main Menu");
        else if (gameObject.name == "Abilities") player.ToggleMenu("Abilities Menu");
        else if (gameObject.name == "Exit") Application.Quit();
        else if (gameObject.name == "Target Air") toggleColor(player.ToggleAirTarget());
        else if (gameObject.name == "Cursor Mode") toggleColor(player.ToggleArchitect());
        else if (gameObject.name == "Select Adjacent") toggleColor(player.ToggleExtend());
        else if (gameObject.name == "Replace Mode") toggleColor(player.ToggleReplace());
        else if (gameObject.name == "Select Matching") toggleColor(player.ToggleExtendMatching());
    }

    private void toggleColor(bool active)
    {
        if(active) GetComponent<Image>().color = new Color32(140, 140, 140, 100);
        else GetComponent<Image>().color = new Color32(255, 255, 255, 100);
    }
}
