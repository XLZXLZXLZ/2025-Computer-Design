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

    //��ʱЧ��չʾ�������˴��ĳ�����ȫ�ּ�����������ֵ����Ϊͨ��
    protected override void OnHit()
    {
        LightLevelManager.Instance.AddLitCount();
        transform.GetChild(0).gameObject.SetActive(true);

        AudioManager.Instance.PlaySe("LightAchievement");
    }

    protected override void OnLeave()
    {
        LightLevelManager.Instance.ReduceLitCount();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
