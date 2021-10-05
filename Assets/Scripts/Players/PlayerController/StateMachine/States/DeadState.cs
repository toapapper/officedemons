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
	public override void OnStartThrow() { }
	public override void OnThrow() { }
	public override void OnRevive(GameObject player) { }

	public override void OnFixedUpdateState() { }


	Color originalColor;
	public override void OnStateEnter()
	{
		Debug.Log("Enters DeadState" + this);
		//Die animation
		originalColor = GetComponent<MeshRenderer>().material.color;
		GetComponent<MeshRenderer>().material.color = Color.black;
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits DeadState" + this);
		GetComponent<MeshRenderer>().material.color = originalColor;
	}
}
