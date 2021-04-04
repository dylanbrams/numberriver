using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField] BGFx bGFx;
    [SerializeField] FGSFx fGSFx;
    [SerializeField] BGConstantFx bGConstantFX;
    [SerializeField] LevelController levelController;
    [SerializeField] Music music;
    [SerializeField] GameObject MusicSlider;
    [SerializeField] GameObject FGSFXSlider;
    [SerializeField] GameObject BGFxSlider;
    [SerializeField] GameObject BGConstantFXSlider;
    [SerializeField] GameObject MusicToggle;
    [SerializeField] GameObject FGSFXToggle;
    [SerializeField] GameObject BGFxToggle;
    [SerializeField] GameObject BGConstantFXToggle;

    private void Awake()
    {
        SetupSliderValues();
        SetupToggleValues();
    }

    private void SetupToggleValues()
    {
        Toggle musicToggle = MusicToggle.GetComponent<Toggle>();
        musicToggle.isOn = levelController.musicOn;
        Toggle fGSFXToggle = FGSFXToggle.GetComponent<Toggle>();
        fGSFXToggle.isOn = levelController.fGSFXOn;
        Toggle bGFxToggle = BGFxToggle.GetComponent<Toggle>();
        bGFxToggle.isOn = levelController.bgFXOn;
        Toggle bGConstantFXToggle = BGConstantFXToggle.GetComponent<Toggle>();
        bGConstantFXToggle.isOn = levelController.bgConstantFXOn;

    }

    private void SetupSliderValues()
    {
        Slider musicSlider = MusicSlider.GetComponent<Slider>();
        musicSlider.value = music.GetVolume();
        Slider fGSFXSlider = FGSFXSlider.GetComponent<Slider>();
        fGSFXSlider.value = fGSFx.GetVolume();
        Slider bGFxSlider = BGFxSlider.GetComponent<Slider>();
        bGFxSlider.value = bGFx.GetVolume();
        Slider bGConstantFXSlider = BGConstantFXSlider.GetComponent<Slider>();
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

    private void startStopAudio(bool value)
    {
        if (value == false)
            levelController.stopAudio();
        else
            levelController.startAudio();
    }
    public void ToggleBGFx(bool value)
    {
        levelController.bgFXOn = value;
        startStopAudio(value);
    }

    public void ToggleBGConstantFx(bool value)
    {
        levelController.bgConstantFXOn = value;
        startStopAudio(value);
    }

    public void ToggleFGSFx(bool value)
    {
        levelController.fGSFXOn = value;
    }

    public void ToggleMusic(bool value)
    {
        levelController.musicOn = value;
        startStopAudio(value);
    }

    public void MusicSliderChange(float value)
    {
        if (music)
            music.SetVolume(value);
    }

    public void FGSFXSliderChange(float value)
    {
        if (fGSFx)
            fGSFx.SetVolume(value);
    }
    public void BGFxSliderChange(float value)
    {
        if (bGFx)
            bGFx.SetVolume(value);
    }
    public void BGConsantFXSliderChange(float value)
    {
        if (bGConstantFX)
            bGConstantFX.SetVolume(value);
    }

}

