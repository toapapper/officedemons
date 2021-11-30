using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Victory : MonoBehaviour
{
    private GameObject[] encounters;
    private bool achievedVictory;
    [SerializeField]
    private GameObject victoryExit;
    [SerializeField] bool isLevel = true;


    private void Start()
    {
        encounters = GameObject.FindGameObjectsWithTag("Encounter");
    }


    private void Update()
    {
        if (isLevel)
        {
            if (!achievedVictory && IsArrayEmpty())
            {
                victoryExit.SetActive(true);
                achievedVictory = true;
            }
        }
    }

    private bool IsArrayEmpty()
    {
        if (encounters == null || encounters.Length == 0) return true;
        for (int i = 0; i < encounters.Length; i++)
        {
            if (encounters[i] != null)
            {
                return false;
            }
        }
        return true;
    }
}
