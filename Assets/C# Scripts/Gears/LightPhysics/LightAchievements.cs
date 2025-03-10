using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAchievements : LightPhysics
{
    private void Awake()
    {
        LightLevelManager.Instance.RegisiterTarget();
        print(gameObject.name);
    }

    //临时效果展示，后续此处改成增加全局计数项，到达最大值则视为通关
    protected override void OnHit()
    {
        LightLevelManager.Instance.AddLitCount();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    protected override void OnLeave()
    {
        LightLevelManager.Instance.ReduceLitCount();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
