//Written by Ossian

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool InGame = false;
    public GameObject OptionsMenu;
    public GameObject FirstSelectedMainMenu;
    [SerializeField] Image BGImage;

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
        BGImage.color = Color.cyan;
        EventSystem.current.SetSelectedGameObject(FirstSelectedMainMenu);
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
