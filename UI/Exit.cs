using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : UI
{
    protected override void OnClick()
    {
        Application.Quit();
    }
}
