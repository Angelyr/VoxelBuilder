using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undo : UI
{
    protected override void OnClick()
    {
        References.player.Undo();
    }
}
