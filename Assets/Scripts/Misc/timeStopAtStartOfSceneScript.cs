using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeStopAtStartOfSceneScript : MonoBehaviour
{
    public int framesPaused = 300;
    protected int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("timeStopCounter++");
        counter++;

        Time.timeScale = 0;
        if(counter >= framesPaused)
        {
            Time.timeScale = 1;
            this.enabled = false;
        }
    }
}
