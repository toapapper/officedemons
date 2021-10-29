using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>
/// The little health and stamina bar visible over the enemies
/// </para>
///   
/// <para>
/// Author: Ossian 
/// </para>
///  
/// </summary>

// Last Edited: 27-10-21

public class EnemyUI : MonoBehaviour
{

    [SerializeField] private Image hpBar;
    [SerializeField] private Image stamBar;

    private Attributes attributes;
    private Transform mainCamera;


    void Start()
    {
        attributes = GetComponentInParent<Attributes>();
        mainCamera = Camera.main.transform;
    }


    void Update()
    {
        //looks better this way
        transform.rotation = mainCamera.rotation;

        float percent = (float)attributes.Health / (float)attributes.StartHealth;
        hpBar.fillAmount = percent;

        percent = (float)attributes.Stamina / (float)attributes.StartStamina;
        stamBar.fillAmount = percent;
    }
}
