using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Character can not do anything during this state, waits for combat turn
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-12
public class CombatWaitState : AbstractPlayerState
{
	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatWaitState" + this);
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits CombatWaitState" + this);
	}
}
