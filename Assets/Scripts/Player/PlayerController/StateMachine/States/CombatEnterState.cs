using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// TODO: Make the character move to correct position before enetering combat
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-12
public class CombatEnterState : AbstractPlayerState
{
	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatEnterState" + this);
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits CombatEnterState" + this);
	}
}
