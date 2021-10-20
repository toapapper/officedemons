using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>
/// Holds an enum of all status effects that can affect players.
/// </para>
///  <para>
///  Author: Kristian
/// </para>
/// </summary>

// Last Edited: 14-10-2021
public class StatusEffects : MonoBehaviour
{
    enum StatusEffect
    {
        Paralysis, // Stun agent
        Poison, // Take damage per turn
        Cripple, // Walk slower
        Burn, // Take damage per turn
        Blindness //Remove player fov aim
    }
}
