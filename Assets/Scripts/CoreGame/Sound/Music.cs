using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Music : MonoBehaviour
{
    [SerializeField] float volume = 0.25f;
    [SerializeField] AudioClip kleineNachtMusic;
    [SerializeField] AudioClip springMusic;
    [SerializeField] AudioClip summerMusic;
    [SerializeField] AudioClip fallMusic;
    [SerializeField] AudioClip winterMusic;
    private bool fadingOut = false;
    private bool fadingIn = false;
    private float fadeoutTime = 2f;

    private AudioSource myAudioSource;
    private void Awake()
    {
        SetUpSingleton();
        myAudioSource = gameObject.GetComponent<AudioSource>();
        myAudioSource.volume = 0;
    }

    public void playSpringMusic()
    {
        playNewSong(springMusic);
    }

    public void playSummerMusic()
    {
        playNewSong(summerMusic);
    }

    public void playFallMusic()
    {
        playNewSong(fallMusic);
    }

    public void playWinterMusic()
    {
        playNewSong(winterMusic);
    }

    public void playKleineNachtMusic()
    {
        playNewSong(kleineNachtMusic);
    }

    private void playNewSong(AudioClip newSong)
    {
        if ((myAudioSource.clip != newSong && fadingOut != true) || (!myAudioSource.isPlaying && fadingIn == false))
        {
            fadingIn = true;
            StartCoroutine(fadeCurrentSongAndStartNew(newSong));
        }
        fadingIn = false;
    }

    private IEnumerator fadeCurrentSongAndStartNew(AudioClip newSong)
    {
        Debug.Log("Fading song for 2 seconds at time" + Time.timeSinceLevelLoad.ToString());
        fadingOut = true;
        if (volume != 0)
        {
            StartCoroutine(AudioFadeOut.FadeOut(myAudioSource, fadeoutTime));
        }
        yield return new WaitForSeconds(fadeoutTime);
        fadingOut = false;
        myAudioSource.volume = volume;
        Debug.Log("Starting New Song, " + newSong.name.ToString() + " At time" + Time.timeSinceLevelLoad.ToString());
        myAudioSource.clip = newSong;
        myAudioSource.Play();

    }

    public void SetVolume(float volumeIn)
    {
        volume = volumeIn;
        if (myAudioSource)
            myAudioSource.volume = volumeIn;
    }

    public float GetVolume()
    {
        return volume;
    }
    public void stopTheMusic()
    {
        StartCoroutine(fadeCurrentSong());
    }
    private IEnumerator fadeCurrentSong()
    {
        fadingOut = true;
        if (volume != 0)
        {
            StartCoroutine(AudioFadeOut.FadeOut(myAudioSource, fadeoutTime));
        }
        if (Time.deltaTime > 0)
            yield return new WaitForSeconds(fadeoutTime);
        else
            yield return new WaitForSecondsRealtime(fadeoutTime);
        fadingOut = false;
        if (volume == 0)
        {
            myAudioSource.Stop();
        }
    }

    private void SetUpSingleton()
    {
        if(FindObjectsOfType(GetType()).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMenuMusic()
    {
        /*
        myAudioSource = gameObject.GetComponent<AudioSource>();
        if (myAudioSource.clip != menuMusic)
        {
            Debug.Log("Starting Menu Music");
            myAudioSource.clip = menuMusic;
            myAudioSource.Play();
        }
        */
    }

    public void PlayGameMusic()
    {
        /*
        myAudioSource = gameObject.GetComponent<AudioSource>();
        if (myAudioSource.clip != gameMusic) 
        { 
            myAudioSource.clip = gameMusic;
            myAudioSource.Play();
        }
        */
    }

}
