using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private Sprite[] portraits;
    [SerializeField] private Image sprite;
    int index;

    public void NewSelection()
    {
        if(index == 0)
        {
            sprite.sprite = portraits[0];
        }
        else if(index == 1)
        {
            sprite.sprite = portraits[1];
        }
        else if(index == 2)
        {
            sprite.sprite = portraits[2];
        }
        else
        {
            sprite.sprite = portraits[3];
        }
    }

    public void SetIndex(int i)
    {
        index = i;
        Debug.Log("Index: " + index);
    }
}
