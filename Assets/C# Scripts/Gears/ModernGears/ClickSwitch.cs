using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSwitch : Switch
{
    public void OnClick()
    {
        var state = !IsOn;

        SwitchState(state);
    }
}
