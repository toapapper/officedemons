using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CombatActionState : AbstractPlayerState
{
	public override void OnMove(CallbackContext context) { }
	public override void OnAttack(CallbackContext context) { }
	public override void OnSpecial(CallbackContext context) { }
	public override void OnPickupThrow(CallbackContext context) { }
	public override void OnRevive(CallbackContext context) { }

	public override void OnFixedUpdateState() { }


	public override void OnStateEnter()
	{
		Debug.Log("Enters OutOfCombatState" + this);
		switch (ChosenAction)
		{
			case TypeOfAction.ATTACK:
				playerMovement.PerformAttack();
				break;
			case TypeOfAction.SPECIALATTACK:
				playerMovement.PerformSpecial();
				break;
			case TypeOfAction.THROW:
				playerMovement.PerformThrow();
				break;
			case TypeOfAction.REVIVE:
				playerMovement.PerformRevive();
				break;
			case TypeOfAction.NOACTION:
				break;
		}
		ChosenAction = TypeOfAction.NOACTION;
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits OutOfCombatState" + this);
	}
}