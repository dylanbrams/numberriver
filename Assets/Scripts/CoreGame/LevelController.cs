using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    bool levelTimerFinished = false;
    Music music;
    BGFx bGFx;
    BGConstantFX bGConstantFX;
    
    // Start is called before the first frame update
    void Start()
    {
        music = FindObjectOfType<Music>();
        bGConstantFX = FindObjectOfType<BGConstantFX>();
        bGFx = FindObjectOfType<BGFx>();
        bGConstantFX.playTheConstantBGFx();
        bGFx.playTheBGFx();
    }

    // Update is called once per frame
    void Update()
    {
        playTheMusic();
    }

    void playTheMusic()
    {
        /*
        if (Time.timeSinceLevelLoad < 10)
        {
            music.playKleineNachtMusic();
        }
        if (10 < Time.timeSinceLevelLoad && Time.timeSinceLevelLoad < 30)
        {
            music.playSpringMusic();
        }
        if (30 < Time.timeSinceLevelLoad && Time.timeSinceLevelLoad < 50)
        {
            music.playSummerMusic();
        }
        if (50 < Time.timeSinceLevelLoad && Time.timeSinceLevelLoad < 70)
        {
            music.playFallMusic();
        }
        if (70 < Time.timeSinceLevelLoad && Time.timeSinceLevelLoad < 90)
        {
            music.playWinterMusic();
        }
        */
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

    private void StopGenerators()
    {
        //Defender[] myGenerators = FindObjectsOfType<Defender>();
        //foreach (Defender spawner in myGenerators)
        //{
        //    spawner.StopGenerateBalance();
        //}

    }
}
