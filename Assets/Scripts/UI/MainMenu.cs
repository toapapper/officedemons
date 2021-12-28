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
    private void Start()
    {
        if(GameObject.Find("PlayerConfigurationManager") != null && InGame == false)
        {
            Destroy(GameObject.Find("PlayerConfigurationManager"));
        }
        AkSoundEngine.PostEvent("Play_RadioMusic", gameObject);
        AkSoundEngine.PostEvent("Play_Ambience", gameObject);
    }
    public void Play()
    {
        SceneManagment.Instance.NextLevel();
        Time.timeScale = 1;
    }
    
    public void Resume()
    {
        GameManager.Instance.Unpause();
    }
    
    public void Options()
    {
        OptionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstSelectedMainMenu);
        Debug.Log(EventSystem.current);
        gameObject.SetActive(false);
    }
   
    public void Quit()
    {
        if (!InGame)
            Application.Quit(1000);
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
        //Application.Quit();
    }

    public void PlayTutorial()
    {
        SceneManagment.Instance.GetLevel(4);
    }
    
}
