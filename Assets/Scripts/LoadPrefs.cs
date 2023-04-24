using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrefs : MonoBehaviour
{
    [Header("AudioOptions")]
    private const string MUSIC_VOLUME = "musicVolume";
    [SerializeField] private Slider volumeSlider;

    [Header("VideoOptions")]
    private const string QUALITY_INDEX = "qualityIndex";
    private const string RESOLUTION_INDEX = "resolutionIndex";
    private const string FULL_SCREEN_TOGGLE = "fullScreenToggle";
    private const string FRAMERATE_INDEX = "framerateIndex";
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown framerateDropdown;
    public List<Resolution> resolutions;

    private void Awake()
    {
        fullScreenToggle.isOn = PlayerPrefs.GetInt(FULL_SCREEN_TOGGLE, 1) == 1;

        if (!PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, 1);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }

        if (!PlayerPrefs.HasKey(QUALITY_INDEX))
        {
            PlayerPrefs.SetInt(QUALITY_INDEX, 0);
            LoadQuality();
        }
        else
        {
            LoadQuality();
        }

        if (!PlayerPrefs.HasKey(FRAMERATE_INDEX))
        {
            PlayerPrefs.SetInt(FRAMERATE_INDEX, 0);
            LoadFramerate();
        }
        else
        {
            LoadFramerate();
        }


        // Resolutions settings

        resolutions = GetResolutions();
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        if (!PlayerPrefs.HasKey(RESOLUTION_INDEX))
        {
            PlayerPrefs.SetInt(RESOLUTION_INDEX, currentResolutionIndex);
            LoadResolution();
        }
        else
        {
            LoadResolution();
        }

        resolutionDropdown.RefreshShownValue();
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME);
    }

    private void LoadQuality()
    {
        graphicsDropdown.value = PlayerPrefs.GetInt(QUALITY_INDEX);
    }

    private void LoadFramerate()
    {
        framerateDropdown.value = PlayerPrefs.GetInt(FRAMERATE_INDEX);
    }

    private void LoadResolution()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt(RESOLUTION_INDEX);
    }

    private List<Resolution> GetResolutions()
    {
        //Filters out all resolutions with low refresh rate:
        Resolution[] resolutions = Screen.resolutions;
        HashSet<Tuple<int, int>> uniqResolutions = new HashSet<Tuple<int, int>>();
        Dictionary<Tuple<int, int>, int> maxRefreshRates = new Dictionary<Tuple<int, int>, int>();
        for (int i = 0; i < resolutions.GetLength(0); i++)
        {
            //Add resolutions (if they are not already contained)
            Tuple<int, int> resolution = new Tuple<int, int>(resolutions[i].width, resolutions[i].height);
            uniqResolutions.Add(resolution);
            //Get framerate:
            if (!maxRefreshRates.ContainsKey(resolution))
            {
                maxRefreshRates.Add(resolution, resolutions[i].refreshRate);
            }
            else
            {
                maxRefreshRates[resolution] = resolutions[i].refreshRate;
            }
        }
        //Build resolution list:
        List<Resolution> uniqResolutionsList = new List<Resolution>(uniqResolutions.Count);
        foreach (Tuple<int, int> resolution in uniqResolutions)
        {
            Resolution newResolution = new Resolution();
            newResolution.width = resolution.Item1;
            newResolution.height = resolution.Item2;
            if (maxRefreshRates.TryGetValue(resolution, out int refreshRate))
            {
                newResolution.refreshRate = refreshRate;
            }
            uniqResolutionsList.Add(newResolution);
        }
        return uniqResolutionsList;
    }

}
