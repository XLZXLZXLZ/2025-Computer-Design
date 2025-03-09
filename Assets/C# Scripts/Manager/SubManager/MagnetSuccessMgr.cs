using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSuccessMgr : MonoBehaviour
{
    [SerializeField] List<MagnetCage> Cages = new List<MagnetCage>();

    bool Success;
    [SerializeField] GameObject Compass;

    Coroutine cor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Success = true;
        foreach (var cage in Cages) {
            if(!cage.success) {
                Success = false;
                break;
            }
        }

        if (Success && cor == null) {
            Compass.GetComponent<HingeJoint2D>().useMotor = true;
            cor = StartCoroutine(Finish());
            //TODO:ÇÐ»»³¡¾°
        }
    }


    IEnumerator Finish() {
        yield return new WaitForSeconds(5.5F);
        LevelManager.Instance.FinishLevel();
    }
}
