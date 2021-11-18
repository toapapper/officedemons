using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>
/// Creates a list of Weapons and their status modifiers
/// </para>
///  <para>
///  Author: Tim & Kristian
/// </para>
/// </summary>

// Last Edited: 21-10-2021


//ChangeLog 20-10-2021
// List<String> is now a WeaponStatsGeneration class
public class WeaponList : MonoBehaviour
{
    public static List<string> weaponNames = new List<string> { "Boring", "Short", "Long", "Light", "Heavy", "Sharp",
        "Dull", "Frail", "Durable", "Stunning","Toxic", "Flaming" };

    /// <summary>
    /// <para>WeaponNames meaning:</para>
    /// <para>Stunning: Adds Paralysis<br/>
    /// Long: Longer reach<br/>
    /// Short: Shorter reach<br/>
    /// Heavy: Takes more stamina and has more knockback<br/>
    /// Light: Takes less stamina and has less knockback<br/>
    /// Dull: Lesser damage to pointy weapons<br/>
    /// Sharp: More damage to pointy weapons<br/>
    /// Frail: Less durability<br/>
    /// Durable: More durability<br/>
    /// Toxic: Adds Poison<br/>
    /// Boring: No effect at all.<br/>
    /// Flaming: Adds burn<br/>
    /// </para>
    /// </summary>
    public static Dictionary<string, WeaponStatsGeneration> weaponDictionary;

    /// <summary>
    /// The order of attributes in each weapon:
    /// Damage
    /// Durability
    /// Range
    /// Weight (knockback resist) (less stamina)
    /// Weapon model (rarity)
    ///     Common
    ///     Uncommon
    ///     Rare
    ///     Epic
    ///     Legendary
    /// Status effects: (color)
    ///     Normal
    ///     Knockback   (More knockback effect)  
    ///     Paralizys   (Can't move next turn)
    ///     Poison      (damage over time)
    ///     Cripple     (costs more stamina)
    ///     Burn        (damage over time)
    ///     Blindness   (very small FOV/No FOV)
    /// </summary>
    private void Start()
    {
        //weaponDictionary = new Dictionary<string, List<string>>();
        weaponDictionary = new Dictionary<string, WeaponStatsGeneration>();
        PopulateDictionary();
    }
    /// <summary>
    /// Populates the Dictinary with weapons with unique attributes.
    /// </summary>
    private void PopulateDictionary()
    {
        int indexCount = 0;
        //Boring:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(1,0,1,1,1,new List<WeaponEffects> {WeaponEffects.Nothing }));
        indexCount++;

        //Short:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(1.5f, 5, .5f, 0, .5f, new List<WeaponEffects> { WeaponEffects.Nothing }));
        indexCount++;

        //Long:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(1, 10,2, 0, 2, new List<WeaponEffects> { WeaponEffects.Nothing }));
        indexCount++;

        //Light:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(1, 0, 1, -2, .1f, new List<WeaponEffects> { WeaponEffects.Nothing }));
        indexCount++;

        //Heavy:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(1.5f, 20, 1, 0, 3, new List<WeaponEffects> { WeaponEffects.Nothing }));
        indexCount++;

        //Sharp:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(2, 0, .5f, -2, 1, new List<WeaponEffects> { WeaponEffects.Nothing }));
        indexCount++;

        //Dull:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(.5f, 10, 1, 0, 1, new List<WeaponEffects> { WeaponEffects.Nothing }));
        indexCount++;

        //Frail:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(1, 0, 1, -4, 1, new List<WeaponEffects> { WeaponEffects.Nothing }));
        indexCount++;

        //Durable:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(1, 0, 1, 5, 1, new List<WeaponEffects> { WeaponEffects.Nothing }));
        indexCount++;

        //Stunning:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(.5f, 10, .5f, -2, 1, new List<WeaponEffects> { WeaponEffects.StaminaDrain }));
        indexCount++;

        //Toxic:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(.5f, 0, .5f, -1, 1, new List<WeaponEffects> { WeaponEffects.Poison }));
        indexCount++;
        //Flaming:
        weaponDictionary.Add(weaponNames[indexCount], new WeaponStatsGeneration(.5f, 0, .5f, -1, 1, new List<WeaponEffects> { WeaponEffects.Fire }));
        indexCount++;
    }
}
