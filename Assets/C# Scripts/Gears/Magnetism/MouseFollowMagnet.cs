using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowMagnet : MonoBehaviour
{
    PointEffector2D pointEffector;
    PickableMagnet current_PickedMagnet;
    Rigidbody2D rb;

    public bool Connected => current_PickedMagnet != null;

    private void Awake()
    {
        pointEffector = GetComponent<PointEffector2D>();        
        pointEffector.enabled = false;
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowPointer();

        if (Input.GetMouseButton(0))
        {
            if(!current_PickedMagnet)
                pointEffector.enabled = true;
            
        } else {
            pointEffector.enabled = false;
            if (current_PickedMagnet != null) {
                current_PickedMagnet.DisConnect();
                current_PickedMagnet = null;
            }
        }
    }

    void FollowPointer() {
        rb.MovePosition(Tools.MousePosition);
    }

    public void ConnectMagnet(PickableMagnet mag) { 
        current_PickedMagnet = mag;
        pointEffector.enabled = false;
    }
}
