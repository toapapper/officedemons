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

// Last Edited: 28-10-2021

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image fireIcon;
    [SerializeField] private Image bleedIcon;
    [SerializeField] private Image poisonIcon;
    [SerializeField] private Image stamDrainIcon;
    [SerializeField] private Image vulnerableIcon;
    [SerializeField] private Image paralyzeIcon;
    [SerializeField] private Image slowIcon;
    [SerializeField] private Image healOTIcon;
    [SerializeField] private Image stamFillIcon;
    [SerializeField] private Image dmgBoostIcon;



    private StatusEffectHandler effects;

    void Start()
    {
        effects = GetComponentInParent<StatusEffectHandler>();
    }

    //Disugusting implementation i know
    void Update()
    {
        fireIcon.enabled = false;
        bleedIcon.enabled = false;
        poisonIcon.enabled = false;
        stamDrainIcon.enabled = false;
        vulnerableIcon.enabled = false;
        paralyzeIcon.enabled = false;
        slowIcon.enabled = false;
        healOTIcon.enabled = false;
        stamFillIcon.enabled = false;
        dmgBoostIcon.enabled = false;

        for(int i = 0; i < (int)StatusEffectType.NumberOfTypes; i++)
        {
            StatusEffectType type = (StatusEffectType)i;

            if (effects.ActiveEffects.ContainsKey(type))
            {
                switch (type)
                {
                    case StatusEffectType.Fire:
                        fireIcon.enabled = true;
                        break;
                    case StatusEffectType.Bleed:
                        bleedIcon.enabled = true;
                        break;
                    case StatusEffectType.Poison:
                        poisonIcon.enabled = true;
                        break;
                    case StatusEffectType.StaminaDrain:
                        stamDrainIcon.enabled = true;
                        break;
                    case StatusEffectType.Vulnerable:
                        vulnerableIcon.enabled = true;
                        break;
                    case StatusEffectType.HealOverTime:
                        healOTIcon.enabled = true;
                        break;
                    case StatusEffectType.StaminaFill:
                        stamFillIcon.enabled = true;
                        break;
                    case StatusEffectType.Paralyze:
                        paralyzeIcon.enabled = true;
                        break;
                    case StatusEffectType.DamageBoost:
                        dmgBoostIcon.enabled = true;
                        break;
                    case StatusEffectType.Slow:
                        slowIcon.enabled = true;
                        break;
                }
            }
        }
    }
}
