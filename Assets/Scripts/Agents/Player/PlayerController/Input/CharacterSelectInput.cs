using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// <para>
/// Iterates through the character portraits, used in character selection screen.
/// </para>
///  <para>
///  Author: Kristian
/// </para>
/// </summary>
/*
 * Last Edited:
 * 12-11-2021
 */
public class CharacterSelectInput : MonoBehaviour
{
    [SerializeField] private GameObject[] portraits;
    int index;

    public void NewSelection(GameObject GO)
    {
        for (int i = 0; i < portraits.Length; i++)
        {
            if(portraits[i] == GO)
            {
                GO.SetActive(true);
            }
            else
            {
                GO.SetActive(false);
            }
        }
    }
}
