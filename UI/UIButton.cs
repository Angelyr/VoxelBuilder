using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        if (GetComponent<Button>())
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
    }

    protected virtual void OnClick() {
        Debug.Log(gameObject.name);

        if (gameObject.name == "Continue") References.player.ToggleMenu();
        else if (gameObject.name == "Exit") Application.Quit();
        else if (gameObject.name == "TargetAir") References.player.ToggleAirTarget();
        else if (gameObject.name == "CursorMode") References.player.ToggleArchitect();
        else if (gameObject.name == "AutoSelectWall") References.player.ToggleExtend();
        else if (gameObject.name == "Replace") References.player.ToggleReplace();
        else if (gameObject.name == "AutoSelectMatching") References.player.ToggleExtendMatching();
    }
}
