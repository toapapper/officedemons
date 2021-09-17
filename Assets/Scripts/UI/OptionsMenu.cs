//written by Ossian

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject FirstSelectedMainMenu;

    public void Back()
    {
        MainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstSelectedMainMenu);
        gameObject.SetActive(false);
    }
}
