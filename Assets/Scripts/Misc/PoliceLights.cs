using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <para>
/// Script to rotate lights on police cars
/// </para><para>
/// Author: Jonas
/// </para>
/// </summary>

// Last Edited: 30/11-21
public class PoliceLights : MonoBehaviour
{
    [SerializeField]
    GameObject rightWall;
    [SerializeField]
    GameObject leftWall;
    [SerializeField]
    GameObject rightLight;
    [SerializeField]
    GameObject leftLight;

    // Update is called once per frame
    void Update()
    {
        rightWall.transform.RotateAround(rightLight.transform.position, Vector3.up, 720 * Time.deltaTime);
        leftWall.transform.RotateAround(leftLight.transform.position, Vector3.up, 720 * Time.deltaTime);
    }
}
