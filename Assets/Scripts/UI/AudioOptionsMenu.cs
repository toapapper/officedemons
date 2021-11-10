using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
