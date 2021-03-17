using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionSettings : MonoBehaviour
{
    [SerializeField] public int polyOrderMin = -4;
    [SerializeField] public int polyOrderMax = 4;
    [SerializeField] public int polyCofactorMin = -1;
    [SerializeField] public int polyCofactorMax = 1;
    [SerializeField] public bool polyIncludeT = true;
    [SerializeField] public bool polyIncludeP = false;

    [SerializeField] public bool trigIncludeInverse = false;
    [SerializeField] public bool trigIncludeT = false;
    [SerializeField] public bool trigIncludeP = true;
    [SerializeField] public bool trigIncludeI = false;
    [SerializeField] public double trigCofactorMin = 1;
    [SerializeField] public double trigCofactorMax = 1;
    [SerializeField] public double trigPeriodMultMin = 1;
    [SerializeField] public double trigPeriodMultMax = 1;

    [SerializeField] public bool metaAddActive = false;
    [SerializeField] public bool metaSubtractActive = false;
    [SerializeField] public bool metaMultiplyActive = true;
    [SerializeField] public bool metaDivideActive = false;
    [SerializeField] public bool metaIntegrateActive = false;
    [SerializeField] public bool metaDeriveActive = false;

}
