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

// Last Edited: 14-10-2021
public class WeaponList : MonoBehaviour
{
    public static List<string> weaponNames = new List<string> { "Stunning", "Long", "Short", "Heavy", "Light", "Dull", "Sharp", "Frail", "Durable", "Toxic", "Boring" };
    /// <summary>
    /// <para>WeaponNames meaning:</para>
    /// <para>Stunning: Adds Paralysis
    /// Long: Longer reach
    /// Short: Shorter reach
    /// Heavy: Takes more stamina and has more knockback
    /// Light: Takes less stamina and has less knockback
    /// Dull: Lesser damage to pointy weapons
    /// Sharp: More damage to pointy weapons
    /// Frail: Less durability
    /// Durable: More durability
    /// Toxic: Adds Poison
    /// Boring: No effect at all.
    /// </para>
    /// </summary>
    public static Dictionary<string, List<string>> weaponDictionary;

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
        weaponDictionary = new Dictionary<string, List<string>>();
        PopulateDictionary();
    }
    /// <summary>
    /// Populates the Dictinary with weapons with unique attributes.
    /// </summary>
    void PopulateDictionary()
    {
        int indexCount = 0;
        //Stunning:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "3", "1", "1", "Paralysis"});
        indexCount++;
        //Long:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "3", "2", "1", "Normal" });
        indexCount++;
        //Short:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "3", "1", "4", "Normal" });
        indexCount++;
        //Heavy:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "5", "1", "1", "Knockback" });
        indexCount++;
        //Light:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "5", "1", "1", "Normal" });
        indexCount++;
        //Dull:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "5", "1", "1", "Normal" });
        indexCount++;
        //Sharp:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "5", "1", "1", "Normal" });
        indexCount++;
        //Frail:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "5", "1", "1", "Normal" });
        indexCount++;
        //Durable:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "5", "1", "1", "Normal" });
        indexCount++;
        //Toxic:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "5", "1", "1", "Poison" });
        indexCount++;
        //Boring:
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "1", "3", "1", "1", "Normal" });
    }
}
