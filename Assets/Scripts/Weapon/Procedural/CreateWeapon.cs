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

// Last Edited: 14-10-2021
public class CreateWeapon : MonoBehaviour
{  
    void Start()
    {
        int rnd = UnityEngine.Random.Range(0, 10);
        string name = WeaponList.weaponNames[rnd];
        GetStats(name);
        Destroy(this);
    }
    /// <summary>
    /// Gets the stats from the randomly generated name in the WeaponList
    /// </summary>
    /// <param name="name">The randomly generated name from a list of named attributes</param>
    void GetStats(string name)
    {
        int rnd = UnityEngine.Random.Range(0, 100);
        string rarity = "";
        List<string> stats;
        if(WeaponList.weaponDictionary.TryGetValue(name, out stats))
        {
            if(rnd < 50)
            {
                rarity = "Common";
                GetComponent<AbstractWeapon>().Damage *= Int32.Parse(stats[0]);
                // Durability GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponent<AbstractWeapon>().ViewDistance = Int32.Parse(stats[2]);
                // Weight GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponentInParent<Outline>().OutlineColor = Color.gray;
            }
            else if(rnd >= 50 && rnd < 75)
            {
                rarity = "Uncommon";
                GetComponent<AbstractWeapon>().Damage *= Int32.Parse(stats[0]);
                // Durability GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponent<AbstractWeapon>().ViewDistance = Int32.Parse(stats[2]);
                // Weight GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponentInParent<Outline>().OutlineColor = Color.green;
            }
            else if(rnd >= 75 && rnd < 90)
            {
                rarity = "Rare";
                GetComponent<AbstractWeapon>().Damage *= Int32.Parse(stats[0]);
                // Durability GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponent<AbstractWeapon>().ViewDistance = Int32.Parse(stats[2]);
                // Weight GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponentInParent<Outline>().OutlineColor = Color.blue;
            }
            else if(rnd >= 90 && rnd < 97)
            {
                rarity = "Epic";
                GetComponent<AbstractWeapon>().Damage *= Int32.Parse(stats[0]);
                // Durability GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponent<AbstractWeapon>().ViewDistance = Int32.Parse(stats[2]);
                // Weight GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponentInParent<Outline>().OutlineColor = Color.magenta;
            }
            else
            {
                rarity = "Legendary";
                GetComponent<AbstractWeapon>().Damage *= Int32.Parse(stats[0]);
                // Durability GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponent<AbstractWeapon>().ViewDistance = Int32.Parse(stats[2]);
                // Weight GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
                GetComponentInParent<Outline>().OutlineColor = new Color(1,0.5f,0,1);
            }
            transform.parent.name = (rarity + " " + name + " " + this.gameObject.name);
        }            
    }
}
