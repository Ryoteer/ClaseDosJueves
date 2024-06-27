using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [Header("<color=orange>Game Object</color>")]
    [SerializeField] private bool _enableOnStart = true;

    [Header("<color=orange>UI</color>")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [Header("<color=orange>Values</color>")]
    [Range(0f, 1f)][SerializeField] private float _initMasterVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float _initMusicVolume = .2f;
    [Range(0f, 1f)][SerializeField] private float _initSFXVolume = .8f;

    private void Start()
    {
        _masterSlider.value = _initMasterVolume;
        SetMasterVolume(_initMasterVolume);
        _musicSlider.value = _initMusicVolume;
        SetMusicVolume(_initMusicVolume);
        _sfxSlider.value = _initSFXVolume;
        SetSFXVolume(_initSFXVolume);

        gameObject.SetActive(_enableOnStart);
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
}
