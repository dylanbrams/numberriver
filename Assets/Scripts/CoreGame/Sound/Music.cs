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
    private bool fading = false;

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
        if (myAudioSource.clip != newSong && fading != true)
        {
            StartCoroutine(fadeCurrentSongAndStartNew(newSong));
        }
    }

    private IEnumerator fadeCurrentSongAndStartNew(AudioClip newSong)
    {
        Debug.Log("Fading song for 2 seconds at time" + Time.timeSinceLevelLoad.ToString());
        fading = true;
        if (volume != 0)
        {
            StartCoroutine(AudioFadeOut.FadeOut(myAudioSource, 2f));
        }
        yield return new WaitForSeconds(2);
        fading = false;
        myAudioSource.volume = volume;
        Debug.Log("Starting New Song, " + newSong.name.ToString() + " At time" + Time.timeSinceLevelLoad.ToString());
        myAudioSource.clip = newSong;
        myAudioSource.Play();

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
