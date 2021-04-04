using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    [SerializeField] public bool musicOn = false;
    [SerializeField] public bool bgFXOn = false;
    [SerializeField] public bool bgConstantFXOn = false;
    [SerializeField] public bool fGSFXOn = false;
    [SerializeField] GameObject GameplayUI;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] MeshRenderer WaveBackgroundMeshRenderer;

    bool levelTimerFinished = false;
    Music music;
    BGFx bGFx;
    FGSFx fGSFx;
    BGConstantFx bGConstantFX;
    Material myMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        setupAudioObjects();
        startAudio();
        setupMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        // playTheMusic();
    }

    void setupAudioObjects()
    {
        music = FindObjectOfType<Music>();
        bGConstantFX = FindObjectOfType<BGConstantFx>();
        bGFx = FindObjectOfType<BGFx>();
        fGSFx = FindObjectOfType<FGSFx>();
    }

    void setupMaterial()
    {
        myMaterial = WaveBackgroundMeshRenderer.material;
    }

    public void startAudio()
    {
        if (bgConstantFXOn)
            bGConstantFX.playConstantBGFx();
        if (bgFXOn)
            bGFx.playBGFX();
        if (musicOn)
            playTheMusic();
        if (fGSFXOn)
            fGSFx.active = true;
    }

    public void stopAudio()
    {
        if (!bgConstantFXOn)
            bGConstantFX.stopConstantBGFx();
        if (!bgFXOn)
            bGFx.stopBGFX();
        if (!musicOn)
            music.stopTheMusic();
        if (!fGSFXOn)
            fGSFx.active = false;

    }
    void playTheMusic()
    {
        music.playSpringMusic();
    }

    public void StopLevel()
    {
        levelTimerFinished = true;
        //StopSpawners();
        //StopGenerators();
    }

    private void StopSpawners()
    {
        FallingFunctionSpawner[] mySpawners = FindObjectsOfType<FallingFunctionSpawner>();
        foreach (FallingFunctionSpawner spawner in mySpawners)
        {
            spawner.StopSpawning();
        }

    }

    private void StartSpawners()
    {
        FallingFunctionSpawner[] mySpawners = FindObjectsOfType<FallingFunctionSpawner>();
        foreach (FallingFunctionSpawner spawner in mySpawners)
        {
            spawner.StartSpawning();
        }

    }

    public bool GetWavesFrozenValue()
    {
        int wavesFrozen = 1;
        if (myMaterial)
            myMaterial.GetInt("FreezeWaves");
        if (wavesFrozen == 0)
            return true;
        else
            return false;
    }
    public void StopWaveBackgroundMotion()
    {
        myMaterial.SetInt("FreezeWaves", 0);
    }

    public void StartWaveBackgroundMotion()
    {
        myMaterial.SetInt("FreezeWaves", 1);
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
        if (!levelTimerFinished)
        {
            StopSpawners();
            StartSpawners();
        }
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
