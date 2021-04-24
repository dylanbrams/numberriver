using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgConstantFx : MonoBehaviour
{
    [SerializeField] float volume = 0.125f;
    [SerializeField] float bgConstantFadeIn = 2f;
    [SerializeField] AudioClip backgroundConstantSound;

    private AudioSource _myAudioSource;
    private void Awake()
    {
        _myAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayConstantBgFx()
    {
        _myAudioSource.volume = 0;
        _myAudioSource.clip = backgroundConstantSound;
        _myAudioSource.Play();
        StartCoroutine(AudioFadeIn.FadeIn(_myAudioSource, bgConstantFadeIn, volume));
    }

    public void SetVolume(float volumeIn)
    {
        if (_myAudioSource)
            _myAudioSource.volume = volumeIn;
        volume = volumeIn;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void StopConstantBgFx()
    {
        _myAudioSource.Stop();
    }

}
