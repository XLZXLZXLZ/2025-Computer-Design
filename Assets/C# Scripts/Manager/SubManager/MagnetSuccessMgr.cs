using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagnetSuccessMgr : MonoBehaviour , ILevelSettlement
{
    [SerializeField] float DoubleStarTimeLimit = 60;
    [SerializeField] float TribleStarTimeLimit = 30;
    float startTime;

    [SerializeField] List<MagnetCage> Cages = new List<MagnetCage>();

    bool Success;
    bool finished = false;
    [SerializeField] GameObject Compass;

    Coroutine cor;


    // Start is called before the first frame update
    void Start()
    {
        Cages = FindObjectsOfType<MagnetCage>().ToList();
        startTime = Time.time;
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

        if (Success && !finished) {
            Settle();
            //TODO:ÇÐ»»³¡¾°
        }
    }

    [HideInInspector]
    public int Score { get; set; }

    [SerializeField] LevelSettlementController _lsc;

    public LevelSettlementController LSC { get => _lsc; }

    public void Settle()
    {
        Debug.Log("Settled");
        if (Time.time - startTime < TribleStarTimeLimit)
            Score = 3;
        else if (Time.time - startTime < DoubleStarTimeLimit)
            Score = 2;
        else Score = 1;
        finished = true;
        LSC.Finished(this);
    }
}
