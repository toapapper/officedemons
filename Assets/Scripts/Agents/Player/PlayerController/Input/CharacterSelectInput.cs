using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
