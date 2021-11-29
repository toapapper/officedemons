using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// <para>
/// Handles navigation in the Audio configurations menu.
/// </para>
///  <para>
///  Author: Kristian
/// </para>
/// </summary>
/*
 * Last Edited:
 * 12-11-2021
 */
public class AudioOptionsMenu : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject firstSelectedOptionsMenu;
    float defaultValue = 50f;
    ChangeVolumeLevels volumeLevels;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider voiceSlider;
    private void Start()
    {
        volumeLevels = GetComponentInChildren<ChangeVolumeLevels>();
        if (volumeLevels.MasterVolume != defaultValue)
        {
            masterSlider.value = volumeLevels.MasterVolume;
            musicSlider.value = volumeLevels.MusicVolume;
            sfxSlider.value = volumeLevels.SFXVolume;
            voiceSlider.value = volumeLevels.VoiceVolume;
        }
        //else
        //{
        //    masterSlider.value = defaultValue;
        //    musicSlider.value = defaultValue;
        //    sfxSlider.value = defaultValue;
        //    voiceSlider.value = defaultValue;
        //}
    }


    public void Back()
    {
        OptionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedOptionsMenu);
        gameObject.SetActive(false);
    }
}
