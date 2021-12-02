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
    private static float masterVolume = 20f;
    private static float musicVolume = 50f;
    private static float sfxVolume = 50f;
    private static float voiceVolume = 50f;

    public float MasterVolume { get { return masterVolume; } set { masterVolume = value; } }
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
    public float SFXVolume { get { return sfxVolume; } set { sfxVolume = value; } }
    public float VoiceVolume { get { return voiceVolume; } set { voiceVolume = value; } }

    public void SetSpecificVolume(string whatValue)
    {
        float sliderValue = thisSlider.value;

        if (whatValue == "Master")
        {
            MasterVolume = thisSlider.value;
            Debug.Log("MasterVolume = " + masterVolume);
            AkSoundEngine.SetRTPCValue("MasterVolume", masterVolume);
        }

        if (whatValue == "Music")
        {
            MusicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
        }

        if (whatValue == "SFX")
        {
            SFXVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("SFXVolume", sfxVolume);
        }

        if (whatValue == "Voices")
        {
            VoiceVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("VoicesVolume", voiceVolume);
        }
    }
}
