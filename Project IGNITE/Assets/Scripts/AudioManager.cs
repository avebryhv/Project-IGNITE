using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public AudioSource sfxPlayer;
    public AudioSource bgmPlayer;

    public AudioClip defaultBGM;
    public bool playBGMOnStart;


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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxPlayer.PlayOneShot(clip, 1);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxPlayer.PlayOneShot(clip, volume);
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }
}
