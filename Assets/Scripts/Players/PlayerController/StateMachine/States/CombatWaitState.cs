using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CombatWaitState : AbstractPlayerState
{
	//public override void OnMove(CallbackContext context) { }
	public override void LockAction() { }
	public override void CancelAction() { }
	public override void OnAttack() { }
	public override void OnSpecial() { }
	public override void OnPickUp(GameObject weapon) { }
	public override void OnStartThrow() { }
	public override void OnThrow() { }
	public override void OnRevive(GameObject player) { }

	public override void OnFixedUpdateState() { }


	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatWaitState" + this);
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits CombatWaitState" + this);
	}
}
