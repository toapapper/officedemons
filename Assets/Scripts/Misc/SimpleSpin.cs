using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spins the object by rotationspeeds around the axees<br/>
/// 
/// author: Ossian
/// </summary>

public class SimpleSpin : MonoBehaviour
{
    [Tooltip("Degrees of rotation per second around the different axes")]
    [SerializeField] private Vector3 rotationSpeeds = new Vector3(36, 36, 36);

    void Update()
    {
        transform.Rotate(rotationSpeeds.x * Time.deltaTime, rotationSpeeds.y * Time.deltaTime , rotationSpeeds.z * Time.deltaTime);
    }
}
