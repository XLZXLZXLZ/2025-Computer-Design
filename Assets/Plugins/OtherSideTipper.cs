using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class OtherSideTipper : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * 100, 120 * Time.deltaTime);
        else
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 240 * Time.deltaTime);
    }
}
