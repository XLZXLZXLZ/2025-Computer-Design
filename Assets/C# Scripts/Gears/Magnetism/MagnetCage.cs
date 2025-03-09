using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCage : MonoBehaviour
{
    public bool success { get; private set; }

    [SerializeField] HingeJoint2D Joint;
    [SerializeField] Rigidbody2D rb;

    private void Awake()
    {
        success = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Init() {
        yield return new WaitForSeconds(1);

        Joint.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PickableMagnet pm = collision.gameObject.GetComponent<PickableMagnet>();
        if (pm) {
            Joint.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;

            pm.ShutDownMagnet();
            success = true;
            Debug.Log("Cagged");
            Joint.useMotor = true;
        }
    }
}
