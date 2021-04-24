using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{

    [SerializeField] GameObject equationHolderSet;
    int _currentScore = 0;
    List<DragAndDropFallingFunctionCell> _cellList;
    TextMeshProUGUI _scoreTMP; // Note: The scoreboard for this points manager needs to be in its children.
    GameTimer _gameTimer;

    public int CurrentScore { get => _currentScore; set => _currentScore = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _scoreTMP = GetComponentInChildren<TextMeshProUGUI>();
        _gameTimer = FindObjectOfType<GameTimer>();
    }

    public void IterateOneScore(float timeFactor)
    {
        FallingFunction[] equationHolders = equationHolderSet.GetComponentsInChildren<FallingFunction>();
        int addToScore = 0;
        foreach (FallingFunction currentContainer in equationHolders)
        {
            Function currentEquation = currentContainer.GETFunction();
            if (currentEquation != null)
            {
                int currentScore = currentEquation.CalculateOnce(_gameTimer.NormalizedTime, CurrentScore);
                currentContainer.DisplayPoints(currentScore);
                addToScore += currentScore;
            }
        }
        _currentScore += Mathf.Max(0, addToScore);
        UpdatePlayerScoreDisplay();
    }

    void UpdatePlayerScoreDisplay()
    {
        _scoreTMP.text = CurrentScore.ToString("D8");
    }
 }
