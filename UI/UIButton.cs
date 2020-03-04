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

    private void Awake()
    {
        if (GetComponent<Button>())
        {
            player = References.player;
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
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
        else if (gameObject.name == "Target Air") player.ToggleAirTarget();
        else if (gameObject.name == "Cursor Mode") player.ToggleArchitect();
        else if (gameObject.name == "Auto Select Wall") player.ToggleExtend();
        else if (gameObject.name == "Replace") player.ToggleReplace();
        else if (gameObject.name == "Auto Select Matching") player.ToggleExtendMatching();
    }
}
