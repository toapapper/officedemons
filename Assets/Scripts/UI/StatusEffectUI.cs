using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>
/// A class that handles the showing of the little icons indicating whether or not you are affected by a status effect
/// </para>
///   
///  <para>
///  Author: Ossian
/// </para>
/// </summary>

// Last Edited: 14-12-2021

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image fireIcon;
    [SerializeField] private Image poisonIcon;
    [SerializeField] private Image iceIcon;
    [SerializeField] private Image vulnerableIcon;
    [SerializeField] private Image hell_fire;
    [SerializeField] private Image hell_poison;
    [SerializeField] private Image hell_ice;



    private StatusEffectHandler effects;

    void Start()
    {
        effects = GetComponentInParent<StatusEffectHandler>();
    }

    //Disugusting implementation i know
    void Update()
    {
        if(transform.parent.tag == "Player")//the enemies has a different script that does this on the same object
        {
            this.transform.rotation = Camera.main.transform.rotation;
        }


        fireIcon.enabled = false;
        poisonIcon.enabled = false;
        iceIcon.enabled = false;
        vulnerableIcon.enabled = false;
        hell_fire.enabled = false;
        hell_poison.enabled = false;
        hell_ice.enabled = false;


        for (int i = 0; i < (int)StatusEffectType.vulnerable; i++)
        {
            StatusEffectType type = (StatusEffectType)i;

            if (effects == null || effects.ActiveEffects == null)//dont know exactly why but i always get one nullreference error otherwise
            {
                return;
            }

            if (effects.ActiveEffects.ContainsKey(type))
            {
                switch (type)
                {
                    case StatusEffectType.fire:
                        fireIcon.enabled = true;
                        break;
                    case StatusEffectType.frost:
                        iceIcon.enabled = true;
                        break;
                    case StatusEffectType.poison:
                        poisonIcon.enabled = true;
                        break;
                    case StatusEffectType.vulnerable:
                        vulnerableIcon.enabled = true;
                        break;
                    case StatusEffectType.hell_fire:
                        hell_fire.enabled = true;
                        break;
                    case StatusEffectType.hell_poison:
                        hell_poison.enabled = true;
                        break;
                    case StatusEffectType.hell_frost:
                        hell_ice.enabled = true;
                        break;
                }
            }
        }
    }
}
