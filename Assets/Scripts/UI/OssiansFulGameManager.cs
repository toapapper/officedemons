//Högst temporär klass, skriven av Ossian

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OssiansFulGameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject FirstButtonSelected;

    bool paused = false;
    

    public void Paus()
    {
        if (!paused)
        {
            Time.timeScale = 0.01f;
            pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FirstButtonSelected);
            paused = true;
        }
        else if (paused)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            paused = false;
        }
    }


}
