using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CombatActionState : AbstractPlayerState
{
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
		Debug.Log("Enters CombatActionState" + this);
		switch (ChosenAction)
		{
			case TypeOfAction.ATTACK:
				weaponHand.Attack();
				break;
			case TypeOfAction.SPECIALATTACK:
				//specialHand.Attack();
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
