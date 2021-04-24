using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Tooltip("The number of seconds the level will last")]
    [SerializeField] float levelTime = 120;
    bool _levelFinished = false;
    float _normalizedTime = 0f;
    int _lastScoreCalc = 0;
    PointsManager _pointsManager;
    TextMeshProUGUI _tValue;

    public float NormalizedTime { get => _normalizedTime; set => _normalizedTime = value; }

    private void Awake()
    {
        _pointsManager = transform.parent.GetComponentInChildren<PointsManager>();
        _tValue = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_levelFinished)
        {
            NormalizedTime = Time.timeSinceLevelLoad / levelTime;
            SetSliderValue();
            CalculateScore();
            ManageLevelFinish();
        }
    }

    private void CalculateScore()
    {
        int currentTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
        if (currentTime - _lastScoreCalc >= 1)
        {
            _pointsManager.IterateOneScore(currentTime);
            _lastScoreCalc = currentTime;
        }
    }

    private void ManageLevelFinish()
    {
        if (NormalizedTime > 1)
        {
            _levelFinished = true;
            FindObjectOfType<LevelController>().StopLevel();
        }

    }
    void SetSliderValue()
    {
        GetComponent<Slider>().value = NormalizedTime;
        _tValue.text = "t = " + NormalizedTime.ToString("n2");
    }

}
