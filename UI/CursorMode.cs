using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMode : UI
{
    protected override void OnClick()
    {
        References.player.ToggleArchitect();
    }
}
