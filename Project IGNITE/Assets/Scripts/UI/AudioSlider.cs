using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public Slider slider;
    public bool isBGM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        if (isBGM)
        {
            if (PlayerPrefs.HasKey("BGMVolume"))
            {
                slider.value = PlayerPrefs.GetFloat("BGMVolume") * 10.0f;
            }
            else
            {
                slider.value = 10;
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                slider.value = PlayerPrefs.GetFloat("SFXVolume") * 10.0f;
            }
            else
            {
                slider.value = 10;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBGMSliderValueChanged()
    {
        float trueVolume = slider.value / 10.0f;
        AudioManager.Instance.SetBGMVolume(trueVolume);
    }

    public void OnSFXSliderValueChanged()
    {
        float trueVolume = slider.value / 10.0f;
        AudioManager.Instance.SetSFXVolume(trueVolume);
    }
}
