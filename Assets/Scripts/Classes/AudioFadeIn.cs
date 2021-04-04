using UnityEngine;
using System.Collections;

public static class AudioFadeIn
{

    public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime, float maxVolume)
    {
        float startVolume = maxVolume * Time.deltaTime / fadeTime;
        audioSource.volume = startVolume;

        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += maxVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

    }

}

