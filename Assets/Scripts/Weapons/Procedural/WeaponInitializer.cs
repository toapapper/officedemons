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

// Last Edited: 16-11-2021
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
    private void GetStats(string name)
    {
        int rarityLowestValue = 0;
        rarityLowestValue = FitnessFunction.Instance.RoomCounter;
        if (rarityLowestValue >= 50)
        {
            rarityLowestValue = 50;
        }       
        int rnd = UnityEngine.Random.Range(rarityLowestValue, rarityLowestValue + 50);
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
                //GetComponentInParent<Outline>().OutlineColor = Color.gray;
            }
            else if(rnd >= 50 && rnd < 75)
            {
                rarity = "Uncommon";
                //If a weapon is uncommon it get's 10% more dmg and range(Not the end value only a 30% more off the value in "stats").
                float uncommonMultiplier = 1.1f;
                GetComponent<AbstractWeapon>().Damage *= stats.damage * uncommonMultiplier;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range * uncommonMultiplier;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                //GetComponentInParent<Outline>().OutlineColor = Color.green;
            }
            else if(rnd >= 75 && rnd < 90)
            {
                rarity = "Rare";
                //If a weapon is rare it get's 10% more dmg and range(Not the end value only a 30% more off the value in "stats").
                //They also have atleast one status.
                float rareMultiplier = 1.1f;
                GetComponent<AbstractWeapon>().Damage *= stats.damage;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                GainSpecialEffect(2, stats);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                //GetComponentInParent<Outline>().OutlineColor = Color.blue;
            }
            else if(rnd >= 90 && rnd < 97)
            {
                rarity = "Epic";
                //If a weapon is epic it get's 30% more dmg and range(Not the end value only a 30% more off the value in "stats").
                //They also have atleast one status.
                float epicMultiplier = 1.3f;
                GetComponent<AbstractWeapon>().Damage *= stats.damage * epicMultiplier;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range * epicMultiplier;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                GainSpecialEffect(2, stats);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                //GetComponentInParent<Outline>().OutlineColor = Color.magenta;
            }
            else
            {
                rarity = "Legendary";
                //If a weapon is Legendary they will get two extra durability
                //They also have atleast two status.
                GetComponent<AbstractWeapon>().Damage *= stats.damage;
                // Durability GetComponent<AbstractWeapon>().Durability += stats.durability + 2;
                GetComponent<AbstractWeapon>().ViewDistance *= stats.range;
                // Weight GetComponent<AbstractWeapon>().Weight *= stats.weight);
                GainSpecialEffect(3, stats);
                //for (int i = 0; i < stats.effects.Count; i++)
                //{
                //    GetComponent<AbstractWeapon>().effectList.add(stats.effetcs[i]);
                //}
                //GetComponentInParent<Outline>().OutlineColor = new Color(1,0.5f,0,1);
            }
            transform.parent.name = GetName(name,rarity, stats);
        }            
    }


    /// <summary>
    /// Add extra effects to the weapon
    /// </summary>
    /// <param name="desiredEffectCount">Write the desired amount of effects <br/> Takes into account already existing effects</param>
    private void GainSpecialEffect(int desiredEffectCount, WeaponStatsGeneration weapon)
    {
        int effectsToAdd = desiredEffectCount - weapon.effects.Count;
        List<StatusEffectType> existingEffects = new List<StatusEffectType>();
        for (int i = 0; i < weapon.effects.Count; i++)
        {
            existingEffects.Add(weapon.effects[i]);
        }
        for (int i = 0; i < effectsToAdd; i++)
        {
            weapon.effects.Add(GetRandomEffect(existingEffects));
        }
    }

    /// <summary>
    /// Get a random effect which isn't already on the weapon
    /// </summary>
    /// <param name="existingEffects">a list of existing effects</param>
    /// <returns></returns>
    private StatusEffectType GetRandomEffect(List<StatusEffectType> existingEffects)
    {
        int maxTries = 20;
        int enumSize = Enum.GetNames(typeof(StatusEffectType)).Length;
        StatusEffectType statusEffect;
        for (int i = 0; i < maxTries; i++)
        {
            bool foundNewStatus = true;
            statusEffect = (StatusEffectType)UnityEngine.Random.Range(0, enumSize -1);
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
        statusEffect = StatusEffectType.none;
        return statusEffect;
    }


    private string GetName(string name, string rarity,  WeaponStatsGeneration weapon)
    {
        System.Text.StringBuilder finalName = new System.Text.StringBuilder();

        finalName.Append(rarity);
        finalName.Append(" ");
        finalName.Append(name);
        finalName.Append(" ");
        finalName.Append(this.gameObject.name);
        if (weapon.effects.Count > 1)
        {
            finalName.Append(" with");
        }
        for (int i = 1; i < weapon.effects.Count; i++)
        {
            if (weapon.effects[i] != StatusEffectType.none)
            {
                finalName.Append(" ");
                finalName.Append(weapon.effects[i]);
            }
        }

        return finalName.ToString();
    }
}
