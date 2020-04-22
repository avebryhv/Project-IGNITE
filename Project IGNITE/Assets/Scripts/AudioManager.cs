using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public AudioSource sfxPlayer;
    public AudioSource bgmPlayer;
    public AudioSource bgmPlayer2;
    public bool fadingInto1;
    public bool fadingInto2;

    public AudioClip defaultBGM;
    public bool playBGMOnStart;

    public float maxBGMVolume;


    public static AudioManager Instance { get => instance; set => instance = value; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playBGMOnStart)
        {
            PlayBGM(defaultBGM);
            bgmPlayer2.Play();
            bgmPlayer.volume = maxBGMVolume;
            bgmPlayer2.volume = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingInto1)
        {
            bgmPlayer.volume += Time.deltaTime;
            bgmPlayer2.volume -= Time.deltaTime;
            bgmPlayer.volume = Mathf.Clamp(bgmPlayer.volume, 0, maxBGMVolume);

            if (bgmPlayer2.volume <= 0)
            {
                fadingInto1 = false;
            }
        }

        if (fadingInto2)
        {
            bgmPlayer.volume -= Time.deltaTime;
            bgmPlayer2.volume += Time.deltaTime;
            bgmPlayer2.volume = Mathf.Clamp(bgmPlayer2.volume, 0, maxBGMVolume);

            if (bgmPlayer.volume <= 0)
            {
                fadingInto2 = false;
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxPlayer.PlayOneShot(clip, 1);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxPlayer.PlayOneShot(clip, volume);
    }

    public void PlaySFX(string path, float volume)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        sfxPlayer.PlayOneShot(clip, volume);
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }

    public void FadeIntoBGM1()
    {
        fadingInto1 = true;
        fadingInto2 = false;
        
    }

    public void FadeIntoBGM2()
    {
        fadingInto2 = true;
        fadingInto1 = false;        
    }
}
