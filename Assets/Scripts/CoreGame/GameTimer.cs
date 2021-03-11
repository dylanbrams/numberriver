using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Tooltip("The number of seconds the level will last")]
    [SerializeField] float levelTime = 40;
    bool levelFinished = false;
    float normalizedTime = 0f;
    int lastScoreCalc = 0;
    PointsManager pointsManager;
    TextMeshProUGUI tValue;

    private void Awake()
    {
        pointsManager = transform.parent.GetComponentInChildren<PointsManager>();
        tValue = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelFinished)
        {
            normalizedTime = Time.timeSinceLevelLoad / levelTime;
            SetSliderValue();
            CalculateScore();
            ManageLevelFinish();
        }
    }

    private void CalculateScore()
    {
        int currentTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
        if (currentTime - lastScoreCalc >= 1)
        {
            pointsManager.IterateOneScore(currentTime);
            lastScoreCalc = currentTime;
        }
    }

    private void ManageLevelFinish()
    {
        if (normalizedTime > 1)
        {
            levelFinished = true;
            FindObjectOfType<LevelController>().StopLevel();
        }

    }
    void SetSliderValue()
    {
        GetComponent<Slider>().value = normalizedTime;
        tValue.text = "t = " + normalizedTime.ToString("n2");
    }

}
