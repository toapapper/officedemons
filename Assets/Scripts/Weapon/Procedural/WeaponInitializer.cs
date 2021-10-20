using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// <para>
/// Creates weapons from the attributes set in WeaponList.
/// </para>
///  <para>
///  Author: Tim & Kristian
/// </para>
/// </summary>

// Last Edited: 20-10-2021
public class WeaponInitializer : MonoBehaviour
{  
    void Start()
    {
        //If a level is created using the PCG level creator, then this script is only called once.
        try
        {
            int rnd = UnityEngine.Random.Range(0, WeaponList.weaponNames.Count - 1);
            string name = WeaponList.weaponNames[rnd];
            GetStats(name);
            Destroy(this);
        }
        catch (NullReferenceException)
        {
            return;
        }
    }
    /// <summary>
    /// Gets the stats from the randomly generated name in the WeaponList
    /// </summary>
    /// <param name="name">The randomly generated name from a list of named attributes</param>
    void GetStats(string name)
    {
        int rnd = UnityEngine.Random.Range(0, 100);
        string rarity = "";
        WeaponStatsGeneration stats;
        if(WeaponList.weaponDictionary.TryGetValue(name, out stats))
        {
            if(rnd < 50)
            {
                rarity = "Common";
                GetComponent<AbstractWeapon>().Damage *= stats.damage;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                GetComponentInParent<Outline>().OutlineColor = Color.gray;
            }
            else if(rnd >= 50 && rnd < 75)
            {
                rarity = "Uncommon";
                GetComponent<AbstractWeapon>().Damage *= stats.damage;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                GetComponentInParent<Outline>().OutlineColor = Color.green;
            }
            else if(rnd >= 75 && rnd < 90)
            {
                rarity = "Rare";
                GetComponent<AbstractWeapon>().Damage *= stats.damage;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                GetComponentInParent<Outline>().OutlineColor = Color.blue;
            }
            else if(rnd >= 90 && rnd < 97)
            {
                rarity = "Epic";
                GetComponent<AbstractWeapon>().Damage *= stats.damage;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                GetComponentInParent<Outline>().OutlineColor = Color.magenta;
            }
            else
            {
                rarity = "Legendary";
                GetComponent<AbstractWeapon>().Damage *= stats.damage;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                GetComponentInParent<Outline>().OutlineColor = new Color(1,0.5f,0,1);
            }
            transform.parent.name = (rarity + " " + name + " " + this.gameObject.name);
        }            
    }


    /// <summary>
    /// Add extra effects to the weapon
    /// </summary>
    /// <param name="desiredEffectCount">Write the desired amount of effects <br/> Takes into account already existing effects</param>
    private void GainSpecialEffect(int desiredEffectCount, WeaponStatsGeneration weapon)
    {
        int effectsToAdd = desiredEffectCount - weapon.effects.Count;
        List<StatusEffect> existingEffects = new List<StatusEffect>();
        for (int i = 0; i < weapon.effects.Count; i++)
        {
            existingEffects.Add(weapon.effects[i]);
        }
        for (int i = 0; i < effectsToAdd; i++)
        {

        }
    }


    private StatusEffect GetRandomEffect(List<StatusEffect> existingEffects)
    {
        int maxTries = 20;
        int enumSize = Enum.GetNames(typeof(StatusEffect)).Length;
        StatusEffect statusEffect;
        for (int i = 0; i < maxTries; i++)
        {
            bool foundNewStatus = true;
            statusEffect = (StatusEffect)UnityEngine.Random.Range(0, enumSize);
            for (int j = 0; j < existingEffects.Count; j++)
            {
                if (statusEffect == existingEffects[i])
                {
                    foundNewStatus = false;
                    break;
                }
            }
            if (foundNewStatus)
            {
                return statusEffect;
            }
            
        }
        //Already has everything so we return nothing
        statusEffect = StatusEffect.Nothing;
        return statusEffect;
    }
}
