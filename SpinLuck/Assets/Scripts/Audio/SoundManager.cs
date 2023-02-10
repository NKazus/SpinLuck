using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    private AudioSource[] _audios;
    private AudioSource _ambientMusic;
    private bool isVibroEnabled = true;

    private void Awake()
    {
        _audios = gameObject.GetComponentsInChildren<AudioSource>();
        foreach(AudioSource audio in _audios)
        {
            switch (audio.gameObject.tag)
            {
                case "Ambient": _ambientMusic = audio; _ambientMusic.loop = true; break;
                default: throw new NotSupportedException();
            }
        }
    }

    public void PlaySource(AudioSource audio)
    {
        try
        {
            ValidateSource(audio);
            audio.Play();
        }
        catch(NotImplementedException exception)
        {
            Debug.Log("Unexpected AudioSource: " + exception);
        }
    }

    public void PlayVibration()
    {
        if (isVibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void ValidateSource(AudioSource audio)
    {
        if(audio == null)
        {
            throw new NotImplementedException();
        }
    }


    #region SETTINGS
    public void SetGlobalVolume(float volumeValue)
    {
        mixerMasterGroup.audioMixer.SetFloat("_MasterVolume", Mathf.Lerp(-80, 0, volumeValue));
    }

    public void SetEffectsVolume(float volumeValue)
    {
        mixerMasterGroup.audioMixer.SetFloat("_EffectsVolume", Mathf.Lerp(-80, 0, volumeValue));
    }

    public void TurnMisic(bool isMusicOn)
    {
        if (isMusicOn)
        {
            mixerMasterGroup.audioMixer.SetFloat("_MusicVolume", 0);
        }
        else
        {
            mixerMasterGroup.audioMixer.SetFloat("_MusicVolume", -80);
        }
    }

    public void TurnVibro(bool isVibroOn)
    {
        isVibroEnabled = isVibroOn;
    }
    #endregion
}
