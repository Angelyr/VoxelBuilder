using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AirTarget : UI
{
    protected override void OnClick()
    {
        References.player.ToggleAirTarget();
    }
}