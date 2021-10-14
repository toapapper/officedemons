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

// Last Edited: 13-10-2021
public class WeaponList : MonoBehaviour
{
    public static List<string> weaponNames = new List<string> { "Stunning", "Long", "Heavy", "Light", "Dull", "Sharp", "Frail", "Toxic", "Durable", "Short" };

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
    ///     Knockback
    ///     Paralizys
    ///     Poison
    ///     Cripple (costs more stamina)
    ///     Fire
    ///     Blindness
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
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "1", "1", "1", "1", "Uncommon", "Paralysis"});
        indexCount++;
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "1", "3", "2", "1", "Common", "Normal" });
        indexCount++;
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "2", "3", "1", "4", "Common", "Knockback" });
        indexCount++;
        weaponDictionary.Add(weaponNames[indexCount], new List<string> { "1", "5", "1", "1", "Common", "Poison" });
    }
}
