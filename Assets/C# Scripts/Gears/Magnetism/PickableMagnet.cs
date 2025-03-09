using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableMagnet : MonoBehaviour
{
    FixedJoint2D Joint;
    Rigidbody2D rb;

    private void Awake()
    {
        Joint = GetComponent<FixedJoint2D>();
        rb = GetComponent<Rigidbody2D>();

        Joint.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MouseFollowMagnet mfm = collision.gameObject.GetComponent<MouseFollowMagnet>();
        if (mfm && !mfm.Connected && Joint) {
            Joint.enabled = true;
            Joint.connectedBody = mfm.GetComponent<Rigidbody2D>();
            mfm.ConnectMagnet(this);
            rb.gravityScale = 0;
        }
    }

    public void DisConnect() {
        Debug.Log("DisConnected");
        if (Joint) {
            Joint.connectedBody = null;
            Joint.enabled = false;
        }
        rb.gravityScale = 1;
    }

    public void ShutDownMagnet() { 
        gameObject.layer = LayerMask.NameToLayer("Default");
        Destroy(Joint);
    }
}
