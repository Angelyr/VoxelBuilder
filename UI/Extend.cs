using UnityEngine;
using System.Collections;

public class Extend : UI
{
    protected override void OnClick()
    {
        References.player.ToggleExtend();
    }
}
