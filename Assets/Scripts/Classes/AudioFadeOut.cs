using UnityEngine;
using System.Collections;

public static class AudioFadeOut
{

    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        float timeChange = 0.2f;
        if (Time.deltaTime > 0)
        {
            timeChange = Time.deltaTime;
        }

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * timeChange / fadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}

