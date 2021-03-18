using UnityEngine;
using System.Collections;

public static class AudioFadeIn
{

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float maxVolume)
    {
        float startVolume = maxVolume * Time.deltaTime / FadeTime;
        audioSource.volume = startVolume;

        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += maxVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

    }

}

