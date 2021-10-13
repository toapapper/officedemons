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

// Last Edited: 13-10-2021
public class CreateWeapon : MonoBehaviour
{  
    void Start()
    {
        int rnd = UnityEngine.Random.Range(0, 3);
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
        List<string> stats;
        if(WeaponList.weaponDictionary.TryGetValue(name, out stats))
        {
            GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
            // Durability GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);
            GetComponent<AbstractWeapon>().ViewDistance = Int32.Parse(stats[2]);
            // Weight GetComponent<AbstractWeapon>().Damage = Int32.Parse(stats[0]);

            this.gameObject.name = (name + " " + this.gameObject.name + " " + stats[4]);
        }            
    }
}
