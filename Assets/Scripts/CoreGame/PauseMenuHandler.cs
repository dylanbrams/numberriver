using System.Collections;
using System.Collections.Generic;
using CoreGame.Sound;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField] BgFx bGFx;
    [SerializeField] FgSFx fGsFx;
    [SerializeField] BgConstantFx bGConstantFX;
    [SerializeField] LevelController levelController;
    [SerializeField] Music music;
    [SerializeField] GameObject musicSlider;
    [SerializeField] GameObject fgsfxSlider;
    [SerializeField] GameObject bgFxSlider;
    [SerializeField] GameObject bgConstantFXSlider;
    [SerializeField] GameObject musicToggle;
    [SerializeField] GameObject fgsfxToggle;
    [SerializeField] GameObject bgFxToggle;
    [SerializeField] GameObject bgConstantFXToggle;

    private void Awake()
    {
        SetupSliderValues();
        SetupToggleValues();
    }

    private void SetupToggleValues()
    {
        Toggle musicToggle = this.musicToggle.GetComponent<Toggle>();
        musicToggle.isOn = levelController.musicOn;
        Toggle fGsfxToggle = fgsfxToggle.GetComponent<Toggle>();
        fGsfxToggle.isOn = levelController.fGsfxOn;
        Toggle bGFxToggle = bgFxToggle.GetComponent<Toggle>();
        bGFxToggle.isOn = levelController.bgFXOn;
        Toggle bGConstantFXToggle = bgConstantFXToggle.GetComponent<Toggle>();
        bGConstantFXToggle.isOn = levelController.bgConstantFXOn;

    }

    private void SetupSliderValues()
    {
        Slider musicSlider = this.musicSlider.GetComponent<Slider>();
        musicSlider.value = music.GetVolume();
        Slider fGsfxSlider = fgsfxSlider.GetComponent<Slider>();
        fGsfxSlider.value = fGsFx.GetVolume();
        Slider bGFxSlider = bgFxSlider.GetComponent<Slider>();
        bGFxSlider.value = bGFx.GetVolume();
        Slider bGConstantFXSlider = bgConstantFXSlider.GetComponent<Slider>();
        bGConstantFXSlider.value = bGConstantFX.GetVolume();
    }

    // Start is called before the first frame update
    public void ToggleFreezeWaveBackground(bool value)
    {
        if (value == true)
            levelController.StopWaveBackgroundMotion();
        else
            levelController.StartWaveBackgroundMotion();

    }

    private void StartStopAudio(bool value)
    {
        if (value == false)
            levelController.StopAudio();
        else
            levelController.StartAudio();
    }
    public void ToggleBgFx(bool value)
    {
        levelController.bgFXOn = value;
        StartStopAudio(value);
    }

    public void ToggleBgConstantFx(bool value)
    {
        levelController.bgConstantFXOn = value;
        StartStopAudio(value);
    }

    public void ToggleFgsFx(bool value)
    {
        levelController.fGsfxOn = value;
    }

    public void ToggleMusic(bool value)
    {
        levelController.musicOn = value;
        StartStopAudio(value);
    }

    public void MusicSliderChange(float value)
    {
        if (music)
            music.SetVolume(value);
    }

    public void FgsfxSliderChange(float value)
    {
        if (fGsFx)
            fGsFx.SetVolume(value);
    }
    public void BgFxSliderChange(float value)
    {
        if (bGFx)
            bGFx.SetVolume(value);
    }
    public void BgConsantFXSliderChange(float value)
    {
        if (bGConstantFX)
            bGConstantFX.SetVolume(value);
    }

}

