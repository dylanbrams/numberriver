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
    private bool _fadingOut = false;
    private bool _fadingIn = false;
    private float _fadeoutTime = 2f;

    private AudioSource _myAudioSource;
    private void Awake()
    {
        SetUpSingleton();
        _myAudioSource = gameObject.GetComponent<AudioSource>();
        _myAudioSource.volume = 0;
    }

    public void PlaySpringMusic()
    {
        PlayNewSong(springMusic);
    }

    public void PlaySummerMusic()
    {
        PlayNewSong(summerMusic);
    }

    public void PlayFallMusic()
    {
        PlayNewSong(fallMusic);
    }

    public void PlayWinterMusic()
    {
        PlayNewSong(winterMusic);
    }

    public void PlayKleineNachtMusic()
    {
        PlayNewSong(kleineNachtMusic);
    }

    private void PlayNewSong(AudioClip newSong)
    {
        if ((_myAudioSource.clip != newSong && _fadingOut != true) || (!_myAudioSource.isPlaying && _fadingIn == false))
        {
            _fadingIn = true;
            StartCoroutine(FadeCurrentSongAndStartNew(newSong));
        }
        _fadingIn = false;
    }

    private IEnumerator FadeCurrentSongAndStartNew(AudioClip newSong)
    {
        Debug.Log("Fading song for 2 seconds at time" + Time.timeSinceLevelLoad.ToString());
        _fadingOut = true;
        if (volume != 0)
        {
            StartCoroutine(AudioFadeOut.FadeOut(_myAudioSource, _fadeoutTime));
        }
        yield return new WaitForSeconds(_fadeoutTime);
        _fadingOut = false;
        _myAudioSource.volume = volume;
        Debug.Log("Starting New Song, " + newSong.name.ToString() + " At time" + Time.timeSinceLevelLoad.ToString());
        _myAudioSource.clip = newSong;
        _myAudioSource.Play();

    }

    public void SetVolume(float volumeIn)
    {
        volume = volumeIn;
        if (_myAudioSource)
            _myAudioSource.volume = volumeIn;
    }

    public float GetVolume()
    {
        return volume;
    }
    public void StopTheMusic()
    {
        StartCoroutine(FadeCurrentSong());
    }
    private IEnumerator FadeCurrentSong()
    {
        _fadingOut = true;
        if (volume != 0)
        {
            StartCoroutine(AudioFadeOut.FadeOut(_myAudioSource, _fadeoutTime));
        }
        if (Time.deltaTime > 0)
            yield return new WaitForSeconds(_fadeoutTime);
        else
            yield return new WaitForSecondsRealtime(_fadeoutTime);
        _fadingOut = false;
        if (volume == 0)
        {
            _myAudioSource.Stop();
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
