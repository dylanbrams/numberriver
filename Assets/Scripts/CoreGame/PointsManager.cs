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
    GameTimer gameTimer;

    public int CurrentScore { get => currentScore; set => currentScore = value; }

    // Start is called before the first frame update
    void Awake()
    {
        scoreTMP = GetComponentInChildren<TextMeshProUGUI>();
        gameTimer = FindObjectOfType<GameTimer>();
    }

    public void IterateOneScore(float timeFactor)
    {
        FallingFunction[] equationHolders = EquationHolderSet.GetComponentsInChildren<FallingFunction>();
        int addToScore = 0;
        foreach (FallingFunction currentContainer in equationHolders)
        {
            Function currentEquation = currentContainer.getFunction();
            if (currentEquation != null)
            {
                int currentScore = currentEquation.CalculateOnce(gameTimer.NormalizedTime, CurrentScore);
                currentContainer.DisplayPoints(currentScore);
                addToScore += currentScore;
            }
        }
        currentScore += Mathf.Max(0, addToScore);
        UpdatePlayerScoreDisplay();
    }

    void UpdatePlayerScoreDisplay()
    {
        scoreTMP.text = CurrentScore.ToString("D8");
    }
 }
