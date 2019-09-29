using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : UI
{
    protected override void OnClick()
    {
        References.player.ToggleMenu();
    }
}
