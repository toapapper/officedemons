using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Enum containing all states player can have
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-12
public enum PlayerStates
{
    OUTOFCOMBAT,
    ENTERCOMBAT,
    COMBATTURN,
    COMBATACTION,
    COMBATWAIT,
	DEAD,
    REVIVE
}
