using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Class used to hold weapon information
/// </para>
///  <para>
///  Author: Tim
/// </para>
/// </summary>

// Last Edited: 22-10-2021
public class WeaponStatsGeneration
{
    public float damage;
    public float knockback;
    public float range;
    public int durability;
    public float weight;
    public List<WeaponEffects> effects;


    /// <summary>
    /// Give the weapon name it's stats
    /// </summary>
    /// <param name="damage">% damage | base * damage</param>
    /// <param name="knockback">how much force will be added</param>
    /// <param name="range">% Range | base * range</param>
    /// <param name="durability">Add a Flat amount</param>
    /// <param name="weight">% Weight | base * weight</param>
    /// <param name="effects">Add all effects you wish to be on the weapon</param>
    public WeaponStatsGeneration(float damage, float knockback, float range, int durability, float weight, List<WeaponEffects> effects)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.range = range;
        this.durability = durability;
        this.weight = weight;
        this.effects = effects;
    }
}
