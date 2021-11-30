using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneManager is already a thing. Handles scene shifts
/// </summary>
public class SceneManagment : MonoBehaviour
{
    //Right now I just set the scene to the corresponding scene but we might want to load in them later if they become bigger
    Scene scene;

    public static SceneManagment Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    public void Restart()
    {
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(scene.buildIndex + 1);
    }

    private void GetLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    private void GetLevel(string levelName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));
    }


}
