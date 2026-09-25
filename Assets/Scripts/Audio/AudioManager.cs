using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("----------- Audio Source -----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------- Background Music -----------")]
    public AudioClip bg_startScreen;
    public AudioClip bg_gameScene;

    [Header("----------- SFX -----------")]
    public AudioClip sfx_pressStart;
    public AudioClip sfx_bonus;
    public AudioClip sfx_lostBonus;


    public static AudioManager instance;
    private bool _muteStatus = false;

    private void Awake()
    {

        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlayBackground(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopBackground()
    {
        musicSource.Stop();
    }

    public bool GetMuteStatus()
    {
        return _muteStatus;
    }

    public void Mute()
    {
        musicSource.mute = true;
        SFXSource.mute = true;
        _muteStatus = true;
    }

    public void Unmute()
    {
        musicSource.mute = false;
        SFXSource.mute = false;
        _muteStatus = false;
    }

}
