using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        if(GameManager.Instance.Platform == Platform.Android)
            Application.targetFrameRate = 90;
    }
}
