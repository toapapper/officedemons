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
    [SerializeField] private Image dmgBoostIcon;
    [SerializeField] private Image hell_fire;
    [SerializeField] private Image hell_poison;
    [SerializeField] private Image hell_ice;
    [SerializeField] private Image glass_cannon;
    [SerializeField] private Image paralyzeIcon;
    [SerializeField] private Image mega_paralyzeIcon;



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
        dmgBoostIcon.enabled = false;
        hell_fire.enabled = false;
        hell_poison.enabled = false;
        hell_ice.enabled = false;
        glass_cannon.enabled = false;
        paralyzeIcon.enabled = false;
        mega_paralyzeIcon.enabled = false;

        //TODO:FIx! (ossian)

        for (int i = 0; i < (int)StatusEffectType.comboAction; i++)
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
                    case StatusEffectType.ice:
                        iceIcon.enabled = true;
                        break;
                    case StatusEffectType.poison:
                        poisonIcon.enabled = true;
                        break;
                    case StatusEffectType.vulnerable:
                        vulnerableIcon.enabled = true;
                        break;
                    case StatusEffectType.damage_boost:
                        dmgBoostIcon.enabled = true;
                        break;
                    case StatusEffectType.hell_fire:
                        hell_fire.enabled = true;
                        break;
                    case StatusEffectType.hell_poison:
                        hell_poison.enabled = true;
                        break;
                    case StatusEffectType.hell_ice:
                        hell_ice.enabled = true;
                        break;
                    case StatusEffectType.glass_cannon:
                        glass_cannon.enabled = true;
                        break;
                    case StatusEffectType.paralysis:
                        paralyzeIcon.enabled = true;
                        break;
                    case StatusEffectType.mega_paralysis:
                        mega_paralyzeIcon.enabled = true;
                        break;
                }
            }
        }
    }
}
