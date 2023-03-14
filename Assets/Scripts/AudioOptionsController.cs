using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsController : MonoBehaviour
{
    private const string MUSIC_VOLUME = "musicVolume";

    [SerializeField] private Slider volumeSlider;

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volumeSlider.value);
    }
}
