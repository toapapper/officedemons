//Högst temporär klass, skriven av Ossian

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OssiansFulGameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject FirstButtonSelected;

    public bool paused = false;
    

    public void Pause()
    {
        if (paused)
            return;

        Time.timeScale = 0.01f;
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstButtonSelected);
        paused = true;
    }

    public void Unpause()
    {
        if (!paused)
            return;

        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        paused = false;
    }

    public void OnPause()
    {
        if (!paused)
            Pause();
        else
            Unpause();
    }

}
