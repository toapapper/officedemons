using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class DeadState : AbstractPlayerState
{
	//public override void OnMove(CallbackContext context) { }
	public override void LockAction() { }
	public override void CancelAction() { }
	public override void OnAttack() { }
	public override void OnSpecial() { }
	public override void OnPickUp(GameObject weapon) { }
	public override void OnThrow(CallbackContext context) { }
	public override void OnRevive(CallbackContext context) { }

	public override void OnFixedUpdateState() { }


	public override void OnStateEnter()
	{
		Debug.Log("Enters DeadState" + this);
		//Die animation

	}

	public override void OnStateExit()
	{
		Debug.Log("Exits DeadState" + this);
	}
}
