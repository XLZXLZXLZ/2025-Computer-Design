using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : Singleton<AudioManager>
{
    protected override bool IsDonDestroyOnLoad => true;

    [SerializeField] private BgmContainer bgmContainer;
    [SerializeField] private SoundEffectContainer soundEffectContainer;

    private Dictionary<string, AudioClip> bgms = new();
    private Dictionary<string, AudioClip> soundEffects = new();

    private GameObject audioRoot;
    private AudioSource bgmComponent;
    private AudioSource seComponent;
    // private List<AudioSource> soundEffectComponents = new();

    public float bgmVolume => bgmComponent.volume;
    public float seVolume => seComponent.volume;
    private void Start()
    {
        bgmContainer.bgms.ForEach(bgm => bgms.Add(bgm.name, bgm));
        soundEffectContainer.soundEffects.ForEach(soundEffect => 
            soundEffects.Add(soundEffect.name, soundEffect));

        audioRoot = Instantiate(new GameObject());
        DontDestroyOnLoad(audioRoot);
        audioRoot.name = "AudioRoot";
        
        bgmComponent = audioRoot.AddComponent<AudioSource>();
        bgmComponent.loop = true;
        seComponent = audioRoot.AddComponent<AudioSource>();

        SetVolume(0.5f, 0.5f);
    }

    public void PlayBgm(string bgmName)
    {
        if (!bgms.ContainsKey(bgmName)) return;
        bgmComponent.clip = bgms[bgmName];
        StartCoroutine(BGMFadeIn(bgmComponent.volume, 1f));
    }

    private IEnumerator BGMFadeIn(float target,float duration)
    {
        bgmComponent.volume = 0f;
        bgmComponent.Play();
        while (bgmComponent.volume < target)
        {
            bgmComponent.volume = Mathf.MoveTowards(bgmComponent.volume, target, 1f * Time.deltaTime / duration);
            yield return null;
        }
    }

    public void StopBgm()
    {
        bgmComponent.Stop();
    }

    public void PlaySe(string seName)
    {
        if (!soundEffects.ContainsKey(seName))
        {
            Debug.LogWarning("音频文件:" + seName + " 暂未导入");
            return;
        }
        seComponent.PlayOneShot(soundEffects[seName]);
    }

    public void SetVolume(float bgmVolume,float seVolume)
    {
        bgmComponent.volume = bgmVolume;
        seComponent.volume = seVolume;
    }
    
    
}
