using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool IsOn { get; private set; }

    public Action SwitchOn;
    public Action SwitchOff;

    public void SwitchState(bool isOn)
    {
        if (!GameManager.Instance.CanInteractNow)
            return;

        if (isOn == IsOn)
            return;
        IsOn = isOn;

        if (IsOn)
            SwitchOn?.Invoke();
        else
            SwitchOff?.Invoke();
    }
}
