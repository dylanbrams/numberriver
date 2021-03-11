using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    [SerializeField] bool UIDisabled;


    public bool GetUIDisabled()
    {
        return UIDisabled;
    }


}
