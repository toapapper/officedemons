//written by Ossian

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject FirstSelectedMainMenu;
    [SerializeField] Image BGImage;


    public void Back()
    {
        MainMenu.SetActive(true);
        BGImage.color = new Color(238, 243, 203, 255);
        EventSystem.current.SetSelectedGameObject(FirstSelectedMainMenu);
        gameObject.SetActive(false);
    }
}
