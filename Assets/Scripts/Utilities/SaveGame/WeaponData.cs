using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Script to save data about weapons
/// </para><para>
/// Author: Jonas and Johan
/// </para>
/// </summary>
[System.Serializable]
public class WeaponData
{
    public float[] position;
    public string weaponName;
    public string weaponType;
    public string wielder;

    public float damage;
    public float hitForce;
    public float throwDamage;
    public float viewDistance;
    public float viewAngle;
    public int durability;
    public float weight;
    public List<StatusEffectType> effects;
    public int bulletsInBurst;

    public float inaccuracy;

    //public float[] outlineColor;

    public WeaponData(GameObject weaponHandle)
    {
        position = new float[3];
        position[0] = weaponHandle.transform.position.x;
        position[1] = weaponHandle.transform.position.y;
        position[2] = weaponHandle.transform.position.z;

        weaponName = weaponHandle.name;
        weaponType = weaponHandle.transform.GetChild(0).name;
        //Debug.Log(weaponType);
        damage = weaponHandle.GetComponentInChildren<AbstractWeapon>().Damage;
        hitForce = weaponHandle.GetComponentInChildren<AbstractWeapon>().HitForce;
        throwDamage = weaponHandle.GetComponentInChildren<AbstractWeapon>().ThrowDamage;
        viewDistance = weaponHandle.GetComponentInChildren<AbstractWeapon>().ViewDistance;
        viewAngle = weaponHandle.GetComponentInChildren<AbstractWeapon>().ViewAngle;
        durability = weaponHandle.GetComponentInChildren<AbstractWeapon>().Durability;
        weight = weaponHandle.GetComponentInChildren<AbstractWeapon>().Weight;
        effects = Utilities.ListDictionaryKeys(weaponHandle.GetComponentInChildren<AbstractWeapon>().EffectList);
        
        if (weaponHandle.GetComponentInChildren<AbstractWeapon>() is BurstShotWeapon)
        {
            bulletsInBurst = weaponHandle.GetComponentInChildren<BurstShotWeapon>().BulletsInBurst;
        }

        if (weaponHandle.GetComponentInChildren<AbstractWeapon>() is RangedWeapon)
        {
            inaccuracy = weaponHandle.GetComponentInChildren<RangedWeapon>().Inaccuracy;
        }
        if (weaponHandle.GetComponentInChildren<AbstractWeapon>().IsHeld)
        {
            wielder = weaponHandle.transform.parent.parent.parent.name;
        }
    }
}
