using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

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
