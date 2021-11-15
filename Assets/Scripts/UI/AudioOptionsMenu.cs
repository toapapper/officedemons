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
    
    public void Back()
    {
        OptionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedOptionsMenu);
        gameObject.SetActive(false);
    }
}
