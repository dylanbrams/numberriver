using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFunctionSpawner : MonoBehaviour
{

    bool spawn = true;
    [SerializeField] float minSpawnRate = .3f;
    [SerializeField] float maxSpawnRate = 2f;
    [SerializeField] FallingFunction fallingFunctionPrefab;
    [SerializeField] Transform fallingFunctionParent;
    public int randomSeed = 65432;
    private float xMin, xMax, yMin, yMax, zLoc;

    private void Awake()
    {
        Vector3[] myRectTransform = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetWorldCorners(myRectTransform);
        xMin = myRectTransform[0].x;
        yMin = myRectTransform[0].y;
        xMax = myRectTransform[2].x;
        yMax = myRectTransform[2].y;
        zLoc = myRectTransform[0].z;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Random.InitState(randomSeed);
        while (spawn == true) 
        {
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
            SpawnFallingFunction();
            //spawn = false;
        }
    }

    public void StopSpawning()
    {
        spawn = false;
    }

    private void SpawnFallingFunction()
    {
        FallingFunction lastFall = Spawn(fallingFunctionPrefab);
        /* Debug.Log("Lastfall X :" + lastFall.transform.position.x + " Lastfall Y: " + lastFall.transform.position.y + 
            " Spawner X :" + transform.position.x + " Spawner Y: " + transform.position.y); */
        SetupFallingFunction(lastFall);
    }

    private FallingFunction Spawn(FallingFunction fallingFunctionPrefab)
    {
        Vector3 randomLoc = getRandomTransformInBoundaries();
        return Instantiate(fallingFunctionPrefab, randomLoc, transform.rotation, fallingFunctionParent);
        
    }

    private Vector3 getRandomTransformInBoundaries()
    {

        return new Vector3(Random.Range(xMin, xMax),
            Random.Range(yMin, yMax), zLoc
            );
    }

    private void SetupFallingFunction(FallingFunction spawnedFunction)
    {
        spawnedFunction.randomIn = Random.value;
        spawnedFunction.SetupFunction();
    }
}
