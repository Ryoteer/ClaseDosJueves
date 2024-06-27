using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("<color=yellow>Audio</color>")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _source;

    public void SetMasterVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    public void PlayClip(AudioClip clip)
    {
        if (_source.clip == clip) return;

        if (_source.isPlaying) _source.Stop();

        _source.clip = clip;

        _source.Play();
    }
}
