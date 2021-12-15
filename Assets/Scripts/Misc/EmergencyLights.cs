using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyLights : MonoBehaviour
{
    [SerializeField]
    Light rightLight;
    [SerializeField]
    Light leftLight;
    [SerializeField]
    List<GameObject> right;
    [SerializeField]
    List<GameObject> left;

    [SerializeField]
    private float blinkingTime = 1;
    private float timeRemaining;


    private void Start()
    {
        timeRemaining = blinkingTime;
    }
    // Update is called once per frame
    void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            rightLight.enabled = !rightLight.enabled;
            leftLight.enabled = !leftLight.enabled;
            foreach(GameObject go in right)
            {
                go.SetActive(!go.activeSelf);
            }
            foreach (GameObject go in left)
            {
                go.SetActive(!go.activeSelf);
            }
            timeRemaining = blinkingTime;
        }
    }


}
