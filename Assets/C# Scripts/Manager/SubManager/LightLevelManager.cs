using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLevelManager : Singleton<LightLevelManager>
{
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

    private void Update()
    {
        print(LitCount);
        if(LitCount >= TargetLitCount && !isFinished)
        {
            timer += Time.deltaTime;
            if (timer > waitTimeToWin)
            {
                isFinished = true;
                LevelManager.Instance.FinishLevel();
            }
        }
        else
        {
            timer = 0;
        }
    }
}
