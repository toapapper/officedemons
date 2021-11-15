using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// <para>
/// Sets volume levels for the different busses in Wwise.
/// </para>
///  <para>
///  Author: Kristian
/// </para>
/// </summary>
/*
 * Last Edited:
 * 12-11-2021
 */
public class ChangeVolumeLevels : MonoBehaviour
{
    [SerializeField] private Slider thisSlider;
    private float masterVolume;
    private float musicVolume;
    private float SFXVolume;
    private float voiceVolume;

    public void SetSpecificVolume(string whatValue)
    {
        float sliderValue = thisSlider.value;

        if (whatValue == "Master")
        {
            masterVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MasterVolume", masterVolume);
        }

        if (whatValue == "Music")
        {
            masterVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
        }

        if (whatValue == "SFX")
        {
            masterVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("SFXVolume", SFXVolume);
        }

        if (whatValue == "Voices")
        {
            masterVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("VoicesVolume", voiceVolume);
        }
    }
}
