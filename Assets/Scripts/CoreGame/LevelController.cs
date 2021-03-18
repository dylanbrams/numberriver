using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    [SerializeField] bool musicOn = false;
    [SerializeField] bool bgFXOn = false;
    [SerializeField] bool bgConstantFXOn = false;
    [SerializeField] GameObject GameplayUI;
    [SerializeField] GameObject PauseMenu;

    bool levelTimerFinished = false;
    Music music;
    BGFx bGFx;
    BGConstantFX bGConstantFX;
    
    // Start is called before the first frame update
    void Start()
    {
        setupAudioObjects();
        startAudio();
    }

    // Update is called once per frame
    void Update()
    {
        // playTheMusic();
    }

    void setupAudioObjects()
    {
        music = FindObjectOfType<Music>();
        bGConstantFX = FindObjectOfType<BGConstantFX>();
        bGFx = FindObjectOfType<BGFx>();
    }

    void startAudio()
    {
        if (bgConstantFXOn)
            bGConstantFX.playConstantBGFx();
        if (bgFXOn)
            bGFx.playBGFX();
        if (musicOn)
            playTheMusic();
    }
    void playTheMusic()
    {
        music.playSpringMusic();
    }

    public void StopLevel()
    {
        levelTimerFinished = true;
        StopSpawners();
        StopGenerators();
    }

    private void StopSpawners()
    {
        FallingFunctionSpawner[] mySpawners = FindObjectsOfType<FallingFunctionSpawner>();
        foreach (FallingFunctionSpawner spawner in mySpawners)
        {
            spawner.StopSpawning();
        }

    }

    public void PauseGame()
    {
        Debug.Log("Pause");
        Time.timeScale = 0;
        GameplayUI.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void UnPauseGame()
    {
        Debug.Log("Unpause");
        Time.timeScale = 1;
        GameplayUI.SetActive(true);
        PauseMenu.SetActive(false);
    }

    private void StopGenerators()
    {
        //Defender[] myGenerators = FindObjectsOfType<Defender>();
        //foreach (Defender spawner in myGenerators)
        //{
        //    spawner.StopGenerateBalance();
        //}

    }
}
