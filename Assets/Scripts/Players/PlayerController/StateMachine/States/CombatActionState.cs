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
		Debug.Log("Enters CombatActionState" + this + " Action: " + GetComponent<CombatTurnState>().ChosenAction);
		switch (GetComponent<CombatTurnState>().ChosenAction)
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
		Debug.LogWarning("Reset action");
		GetComponent<CombatTurnState>().ChosenAction = TypeOfAction.NOACTION;
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits CombatActionState" + this);
	}
}
