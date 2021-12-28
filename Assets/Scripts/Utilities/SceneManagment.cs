using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// SceneManager is already a thing. Handles scene shifts
/// </summary>
public class SceneManagment : MonoBehaviour
{
    //Right now I just set the scene to the corresponding scene but we might want to load in them later if they become bigger
    Scene scene;
    [SerializeField] private GameObject loadingScreen;
    private Image progressBar;
    private float target;
    public static SceneManagment Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        progressBar = loadingScreen.GetComponentInChildren<Image>();
    }

    public void Restart()
    {
        SceneManager.LoadScene(scene.buildIndex);
    }

    public async void NextLevel()
    {
        var nextScene = SceneManager.LoadSceneAsync(scene.buildIndex +1);
        nextScene.allowSceneActivation = false;
        
        loadingScreen.SetActive(true);

        do
        {
            await Task.Delay(100);
            target = nextScene.progress;
        } while (nextScene.progress < 0.9f);

        await Task.Delay(1000);

        nextScene.allowSceneActivation = true;
        loadingScreen.SetActive(false);
    }

    public void GetMainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void GetLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    private void GetLevel(string levelName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));
    }

    private void Update()
    {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
    }

}
