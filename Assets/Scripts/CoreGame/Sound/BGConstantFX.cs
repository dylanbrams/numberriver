using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGConstantFx : MonoBehaviour
{
    [SerializeField] float volume = 0.125f;
    [SerializeField] float BGConstantFadeIn = 2f;
    [SerializeField] AudioClip backgroundConstantSound;

    private AudioSource myAudioSource;
    private void Awake()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void playConstantBGFx()
    {
        myAudioSource.volume = 0;
        myAudioSource.clip = backgroundConstantSound;
        myAudioSource.Play();
        StartCoroutine(AudioFadeIn.FadeIn(myAudioSource, BGConstantFadeIn, volume));
    }

    public void SetVolume(float volumeIn)
    {
        if (myAudioSource)
            myAudioSource.volume = volumeIn;
        volume = volumeIn;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void stopConstantBGFx()
    {
        myAudioSource.Stop();
    }

}
