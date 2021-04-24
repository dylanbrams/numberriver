using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FgSFx : MonoBehaviour
{
    public bool active = true;
    [SerializeField] public float volume = 0.65f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetVolume(float volumeIn)
    {
        volume = volumeIn;
    }

    public float GetVolume()
    {
        return volume;
    }

}
