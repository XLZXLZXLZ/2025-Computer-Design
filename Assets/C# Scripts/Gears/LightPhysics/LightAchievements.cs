using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAchievements : LightPhysics
{

    //��ʱЧ��չʾ�������˴��ĳ�����ȫ�ּ�����������ֵ����Ϊͨ��
    protected override void OnHit()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    protected override void OnLeave()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
