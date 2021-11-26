//written by Ossian

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject VideoOptionMenu;
    public GameObject AudioOptionMenu;
    public GameObject ControllsOptionMenu;
    public GameObject FirstSelectedOptionMenu;
    public GameObject FirstSelectedVideoMenu;
    public GameObject FirstSelectedAudioMenu;
    public GameObject FirstSelectedControllerMenu;

    public void Video()
    {
        VideoOptionMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstSelectedVideoMenu);
        gameObject.SetActive(false);
    }

    public void Audio()
    {
        AudioOptionMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstSelectedAudioMenu);
        gameObject.SetActive(false);
    }

    public void Controls()
    {
        ControllsOptionMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstSelectedControllerMenu);
        gameObject.SetActive(false);
    }

    public void Back()
    {
        MainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstSelectedOptionMenu);
        gameObject.SetActive(false);
    }
}
