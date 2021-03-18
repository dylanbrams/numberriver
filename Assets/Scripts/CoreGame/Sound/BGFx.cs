using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFx : MonoBehaviour
{
    [SerializeField] AudioClip[] backgroundSound;
    [SerializeField] int minPauseBetweenSounds = 10;
    [SerializeField] int maxPauseBetweenSounds = 30;
    [SerializeField] public float BGFxVolume;

    private bool makeSounds = true;

    private AudioSource myAudioSource;

    private void Awake()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>();
        myAudioSource.volume = BGFxVolume;
    }

    public void playBGFX()
    {
        makeSounds = true;
        StartCoroutine(repeatPlayBGFX());
    }

    public void stopBGFX()
    {
        makeSounds = false;
    }

    private IEnumerator repeatPlayBGFX()
    {
        while (makeSounds == true)
        {
            yield return new WaitForSeconds(Random.Range(minPauseBetweenSounds, maxPauseBetweenSounds));
            PlayOneSound();
            //spawn = false;
        }
    }

    private void PlayOneSound()
    {
        myAudioSource.clip = backgroundSound[Random.Range(1, backgroundSound.Length)];
        myAudioSource.Play();

    }

}
