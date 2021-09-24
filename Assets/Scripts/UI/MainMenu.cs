//Written by Ossian

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public bool InGame = false;
    public GameObject OptionsMenu;
    public GameObject FirstSelectedOptionsMenu;

    public void Play()
    {
        SceneManager.LoadScene("OssianScene");
    }
    

    public void Resume()
    {
        GameManager.Instance.Unpause();
    }
    

    public void Options()
    {
        OptionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstSelectedOptionsMenu);
        Debug.Log(EventSystem.current);
        gameObject.SetActive(false);
    }
    

    public void Quit()
    {
        if (!InGame)
            Application.Quit(1000);
        else
            SceneManager.LoadScene("Main Menu");
    }
    
}
