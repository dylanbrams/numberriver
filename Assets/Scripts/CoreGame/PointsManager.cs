using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{

    [SerializeField] GameObject EquationHolderSet;
    int currentScore = 0;
    List<DragAndDropFallingFunctionCell> CellList;
    TextMeshProUGUI scoreTMP; // Note: The scoreboard for this points manager needs to be in its children.

    public int CurrentScore { get => currentScore; set => currentScore = value; }

    // Start is called before the first frame update
    void Awake()
    {
        scoreTMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void IterateOneScore(float timeFactor)
    {
        FallingFunction[] equationHolders = EquationHolderSet.GetComponentsInChildren<FallingFunction>();
        foreach (FallingFunction currentContainer in equationHolders)
        {
            Function currentEquation = currentContainer.getFunction();
            if (currentEquation != null)
            {
                CurrentScore += currentEquation.CalculateOnce(Time.realtimeSinceStartup, CurrentScore);
            }
        }
        UpdatePlayerScore();
    }

    void UpdatePlayerScore()
    {
        scoreTMP.text = CurrentScore.ToString("D8");
    }
 }
