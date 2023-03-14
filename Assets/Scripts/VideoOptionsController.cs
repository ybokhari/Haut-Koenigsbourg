using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class VideoOptionsController : MonoBehaviour
{
    [SerializeField] private LoadPrefs loadPref;

    private const string QUALITY_INDEX = "qualityIndex";
    private const string RESOLUTION_INDEX = "resolutionIndex";
    private const string FULL_SCREEN_TOGGLE = "fullScreenToggle";
    private const string FRAMERATE_INDEX = "framerateIndex";

    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown framerateDropdown;

    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private PostProcessProfile brightness;
    [SerializeField] private PostProcessLayer layer;

    AutoExposure exposure;

    private void Start()
    {
        brightness.TryGetSettings(out exposure);
        SetBrightness(brightnessSlider.value);
    }

    public void SetBrightness(float value)
    {
        if(value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = .05f;
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt(FULL_SCREEN_TOGGLE, isFullScreen ? 1 : 0);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt(QUALITY_INDEX, graphicsDropdown.value);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = loadPref.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(RESOLUTION_INDEX, resolutionIndex);
    }

    public void SetFramerate(int framerateIndex)
    {
        switch (framerateIndex)
        {
            case 0:
                Application.targetFrameRate = -1;
                break;
            case 1:
                Application.targetFrameRate = 144;
                break;
            case 2:
                Application.targetFrameRate = 60;
                break;
            default:
                break;
        }
        PlayerPrefs.SetInt(FRAMERATE_INDEX, framerateIndex);
    }
}
