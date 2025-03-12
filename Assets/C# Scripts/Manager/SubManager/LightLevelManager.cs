using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLevelManager : Singleton<LightLevelManager> , ILevelSettlement
{
    [SerializeField] float DoubleStarTimeLimit = 60;
    [SerializeField] float TribleStarTimeLimit = 30;
    float startTime;

    public int TargetLitCount { get; private set; }

    private int LitCount;

    private bool isFinished = false;

    private float timer = 0f;

    [SerializeField]
    private float waitTimeToWin = 2f; 

    public void RegisiterTarget()
    {
        TargetLitCount++;
    }

    public void AddLitCount()
    {
        LitCount++;
    }

    public void ReduceLitCount()
    {
        LitCount--;
    }

    void Start() { 
        _lsc = FindObjectOfType<LevelSettlementController>();
        startTime = Time.time;
    }

    private void Update()
    {
        //print(LitCount);
        if(LitCount >= TargetLitCount && !isFinished)
        {
            timer += Time.deltaTime;
            if (timer > waitTimeToWin)
            {
                Settle();
            }
        }
        else
        {
            timer = 0;
        }
    }


    [HideInInspector]
    public int Score { get; set; }
    [SerializeField] LevelSettlementController _lsc;

    public LevelSettlementController LSC { get => _lsc; }

    public void Settle()
    {
        if(Time.time - startTime < TribleStarTimeLimit) 
            Score = 3;
        else if(Time.time - startTime < DoubleStarTimeLimit) 
            Score = 2;
        else Score = 1;
        isFinished = true;
        LSC.Finished(this);
    }

}
