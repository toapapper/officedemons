using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    [SerializeField]
    Light rightLight;
    [SerializeField]
    Light leftLight;
    [SerializeField]
    List<GameObject> lamp;

    private float blinkingTime = 2;
    private float timeRemaining;


    private void Start()
    {
        timeRemaining = blinkingTime;
        CameraShake.Shake(2, 0.005f);
    }
    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            rightLight.enabled = !rightLight.enabled;
            leftLight.enabled = !leftLight.enabled;
            foreach (GameObject go in lamp)
            {
                go.SetActive(!go.activeSelf);
            }
            //GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 9.8f);

            timeRemaining = blinkingTime;
            CameraShake.Shake(2, 0.005f);
        }
        if (timeRemaining < 0.4f && timeRemaining > 0.2f)
        {
            rightLight.enabled = !rightLight.enabled;
            leftLight.enabled = !leftLight.enabled;
            foreach (GameObject go in lamp)
            {
                go.SetActive(!go.activeSelf);
            }
        }
        if (timeRemaining < 0.2f && timeRemaining > 0)
        {
            rightLight.enabled = !rightLight.enabled;
            leftLight.enabled = !leftLight.enabled;
            foreach (GameObject go in lamp)
            {
                go.SetActive(!go.activeSelf);
            }
        }
    }
}
