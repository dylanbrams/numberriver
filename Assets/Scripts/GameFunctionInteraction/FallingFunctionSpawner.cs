using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFunctionSpawner : MonoBehaviour
{

    bool _spawn = true;
    [SerializeField] float minSpawnRate = .3f;
    [SerializeField] float maxSpawnRate = 2f;
    [SerializeField] FallingFunction fallingFunctionPrefab;
    [SerializeField] Transform fallingFunctionParent;
    public int randomSeed = 65432;
    private float _xMin, _xMax, _yMin, _yMax, _zLoc;

    private void Awake()
    {
        Vector3[] myRectTransform = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetWorldCorners(myRectTransform);
        _xMin = myRectTransform[0].x;
        _yMin = myRectTransform[0].y;
        _xMax = myRectTransform[2].x;
        _yMax = myRectTransform[2].y;
        _zLoc = myRectTransform[0].z;
    }

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(randomSeed);
        StartSpawning();
    }

    public void StartSpawning()
    {
        _spawn = true;
        StartCoroutine(LoopSpawning());
    }

    private IEnumerator LoopSpawning()
    {
        while (_spawn == true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
            SpawnFallingFunction();
            //spawn = false;
        }

    }

    public void StopSpawning()
    {
        _spawn = false;
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
        Vector3 randomLoc = GETRandomTransformInBoundaries();
        return Instantiate(fallingFunctionPrefab, randomLoc, transform.rotation, fallingFunctionParent);
        
    }

    private Vector3 GETRandomTransformInBoundaries()
    {

        return new Vector3(Random.Range(_xMin, _xMax),
            Random.Range(_yMin, _yMax), _zLoc
            );
    }

    private void SetupFallingFunction(FallingFunction spawnedFunction)
    {
        spawnedFunction.randomIn = Random.value;
        spawnedFunction.SetupFunction();
    }
}
