using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CreateWeapon : MonoBehaviour
{  
    // Start is called before the first frame update
    void Start()
    {
        int rnd = UnityEngine.Random.Range(0, 3);
        string name = WeaponList.weaponNames[rnd];
        GetStats(name);
        Destroy(this);
    }

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
