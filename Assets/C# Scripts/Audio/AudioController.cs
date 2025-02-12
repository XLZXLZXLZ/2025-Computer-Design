using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource BgmAudio;
    [SerializeField] AudioSource SfxAudio;

    public AudioClip bgm;
    public AudioClip wood_break;


    private void Start()
    {
        //加背景音乐，后续瞧瞧情况
/*        BgmAudio.clip=bgm;
        BgmAudio.Play();*/
    }
    public void PlaySfx(AudioClip clip)
    {
        SfxAudio.PlayOneShot(clip);
    }



}
