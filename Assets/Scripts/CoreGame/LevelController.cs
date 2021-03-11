using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    bool levelTimerFinished = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopLevel()
    {
        levelTimerFinished = true;
        StopSpawners();
        StopGenerators();
    }

    private void StopSpawners()
    {
        //AttackerSpawner[] mySpawners = FindObjectsOfType<AttackerSpawner>();
        //foreach (AttackerSpawner spawner in mySpawners)
        //{
        //    spawner.StopSpawning();
        //}

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
