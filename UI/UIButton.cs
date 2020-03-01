using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    private Button button;
    private Player player;

    private void Awake()
    {
        if (!GetComponent<Button>()) return;
        player = References.player;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        if (gameObject.name == "Continue") player.ToggleMenu("Main Menu");
        else if (gameObject.name == "Abilities") player.ToggleMenu("Abilities Menu");
        else if (gameObject.name == "Exit") Application.Quit();
        else if (gameObject.name == "TargetAir") player.ToggleAirTarget();
        else if (gameObject.name == "CursorMode") player.ToggleArchitect();
        else if (gameObject.name == "AutoSelectWall") player.ToggleExtend();
        else if (gameObject.name == "Replace") player.ToggleReplace();
        else if (gameObject.name == "AutoSelectMatching") player.ToggleExtendMatching();
    }
}
