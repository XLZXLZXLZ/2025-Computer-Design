using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Unity.VisualScripting.Member;

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

    [SerializeField] public float bgmVolume =0.25f;
    [SerializeField] public float seVolume =0.4f;


    private float bgmFadeDuration = 1.0f; // 淡入/淡出时间
    private bool isPlaying = false; // 标记是否正在播放

    protected override void Awake()
    {
        base.Awake();

        // 加载或创建音频容器
        bgmContainer = Resources.Load<BgmContainer>("AudioClips/BgmContainer") ?? ScriptableObject.CreateInstance<BgmContainer>();
        soundEffectContainer = Resources.Load<SoundEffectContainer>("AudioClips/SoundEffectContainer") ?? ScriptableObject.CreateInstance<SoundEffectContainer>();

        // 动态加载BGM音频并注入容器
        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("AudioClips/Bgm");
        bgmContainer.bgms.Clear();
        bgmContainer.bgms.AddRange(bgmClips);

        // 动态加载音效音频并注入容器
        AudioClip[] seClips = Resources.LoadAll<AudioClip>("AudioClips/SoundEffect");
        soundEffectContainer.soundEffects.Clear();
        soundEffectContainer.soundEffects.AddRange(seClips);

        // 建立音频字典
        bgmContainer.bgms.ForEach(bgm => bgms.Add(bgm.name, bgm));
        soundEffectContainer.soundEffects.ForEach(soundEffect =>
            soundEffects.Add(soundEffect.name, soundEffect));

        // 初始化音频系统
        audioRoot = new GameObject("AudioRoot");
        DontDestroyOnLoad(audioRoot);

        bgmComponent = audioRoot.AddComponent<AudioSource>();
        bgmComponent.loop = true;
        seComponent = audioRoot.AddComponent<AudioSource>();

        SetVolume(0.25f, 0.25f);


    }


    private void Start()
    {
        // 开始随机播放BGM
        StartCoroutine(RandomBgmPlayer());
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

    //考虑到目前游戏体量不支持一首BGM完整播放，所以采取 BGM在游戏的整个生命周期内持续播放，不会因场景切换而中断 的方案

    //随机播放BGM列表里的BGM，完全由AudioManager控制，不随场景、关卡变化
    private IEnumerator RandomBgmPlayer()
    {

        while (true)
        {
            if (!isPlaying)
            {
                PlayRandomBgm();
            }

            // 等待当前BGM播放结束
            yield return new WaitUntil(() => !bgmComponent.isPlaying);
        }
    }

    private void PlayRandomBgm()
    {
        if (bgms.Count == 0) return;

        string randomBgmName = bgms.Keys.ElementAt(UnityEngine.Random.Range(0, bgms.Count));
        AudioClip randomBgm = bgms[randomBgmName];

        // 淡出当前BGM
        StartCoroutine(FadeOutBgm(bgmComponent, bgmFadeDuration));

        // 播放下一首BGM
        StartCoroutine(PlayNextBgm(randomBgm));
    }

    private IEnumerator PlayNextBgm(AudioClip nextBgm)
    {
        yield return new WaitForSeconds(bgmFadeDuration); // 等待淡出完成
        bgmComponent.clip = nextBgm;
        StartCoroutine(FadeInBgm(bgmComponent, bgmVolume, bgmFadeDuration));
    }

    private IEnumerator FadeInBgm(AudioSource source, float targetVolume, float duration)
    {
        isPlaying = true;
        source.volume = 0f;
        source.Play();

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            source.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        source.volume = targetVolume;
        isPlaying = false;
    }

    private IEnumerator FadeOutBgm(AudioSource source, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            source.volume = Mathf.Lerp(source.volume, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
    }



    //下面暂时没用到，留着以后看看有不有用
    public void PlayBgm()
    {
        /*        if (!bgms.ContainsKey(bgmName)) return;
                bgmComponent.clip = bgms[bgmName];*/

        StartCoroutine(RandomBgmPlayer());
    }


    private IEnumerator BGMFadeIn(float target, float duration)
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

}
