using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BgmContainer",fileName = "BgmContainer")]
public class BgmContainer : ScriptableObject
{
    public List<AudioClip> bgms = new();
    
    public void AddBgm(AudioClip audioClip)
    {
        foreach (var item in bgms)
            if (item.name == audioClip.name) return;
        bgms.Add(audioClip);
    }
}
