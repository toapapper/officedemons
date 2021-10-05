using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

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
