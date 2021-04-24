using System.Collections;
using System.Collections.Generic;
using CoreGame.Sound;
using UnityEngine;

public class LevelController : MonoBehaviour 
{

    [SerializeField] public bool musicOn = false;
    [SerializeField] public bool bgFXOn = false;
    [SerializeField] public bool bgConstantFXOn = false;
    [SerializeField] public bool fGsfxOn = false;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] MeshRenderer waveBackgroundMeshRenderer;

    bool _levelTimerFinished = false;
    private Music _music;
    private BgFx _bGFx;
    private FgSFx _fGSFx;
    private BgConstantFx _bGConstantFX;
    private Material _myMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupAudioObjects();
        StartAudio();
        SetupMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        // playTheMusic();
    }

    void SetupAudioObjects()
    {
        _music = FindObjectOfType<Music>();
        _bGConstantFX = FindObjectOfType<BgConstantFx>();
        _bGFx = FindObjectOfType<BgFx>();
        _fGSFx = FindObjectOfType<FgSFx>();
    }

    void SetupMaterial()
    {
        _myMaterial = waveBackgroundMeshRenderer.material;
    }

    public void StartAudio()
    {
        if (bgConstantFXOn)
            _bGConstantFX.PlayConstantBgFx();
        if (bgFXOn)
            _bGFx.PlayBgfx();
        if (musicOn)
            PlayTheMusic();
        if (fGsfxOn)
            _fGSFx.active = true;
    }

    public void StopAudio()
    {
        if (!bgConstantFXOn)
            _bGConstantFX.StopConstantBgFx();
        if (!bgFXOn)
            _bGFx.StopBgfx();
        if (!musicOn)
            _music.StopTheMusic();
        if (!fGsfxOn)
            _fGSFx.active = false;

    }
    void PlayTheMusic()
    {
        _music.PlaySpringMusic();
    }

    public void StopLevel()
    {
        _levelTimerFinished = true;
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
        if (_myMaterial)
            _myMaterial.GetInt("FreezeWaves");
        if (wavesFrozen == 0)
            return true;
        else
            return false;
    }
    public void StopWaveBackgroundMotion()
    {
        _myMaterial.SetInt("FreezeWaves", 0);
    }

    public void StartWaveBackgroundMotion()
    {
        _myMaterial.SetInt("FreezeWaves", 1);
    }

    public void PauseGame()
    {
        Debug.Log("Pause");
        Time.timeScale = 0;
        gameplayUI.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void UnPauseGame()
    {
        Debug.Log("Unpause");
        Time.timeScale = 1;
        gameplayUI.SetActive(true);
        pauseMenu.SetActive(false);
        if (!_levelTimerFinished)
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
