using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// <para>
/// Handle character actions during combat turn
/// </para>
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-30

public class CombatTurnState : AbstractPlayerState
{
	public override void OnAttack()
	{
		weaponHand.ToggleAimView(true);
		weaponHand.StartAttack();
	}

	public override void OnStartBombard()
	{
		weaponHand.ToggleAimView(true);
		weaponHand.StartBombard();
	}
	//public override bool OnBombard()
	//{
	//	if (IsActionTriggered)
	//	{
	//		return true;
	//	}
	//	return false;
	//}

	public override void OnSpecial()
	{
		specialHand.ToggleAimView(true);
		specialHand.StartAttack();
	}
	public override void OnStartSpecialBombard()
	{
		specialHand.ToggleAimView(true);
		specialHand.StartAttack();
	}

	public override void OnPickUp(GameObject weapon)
	{
		weaponHand.Equip(weapon);
	}

	public override void OnStartThrow()
	{
		weaponHand.StartThrow();
	}
	public override void OnThrow()
	{
		LockAction();
	}

	//public override void OnRevive(GameObject player)
	//{
	//	PlayerToRevive = player;
	//	inputHandler.PlayerToRevive = player;
	//}

	//Lock action
	public override void LockAction()
	{
		switch (inputHandler.ChosenAction)
		{
			case TypeOfAction.ATTACK:
			case TypeOfAction.BOMBARD:
				weaponHand.ToggleAimView(false);
				break;
			case TypeOfAction.SPECIALATTACK:
			case TypeOfAction.SPECIALBOMBARD:
				specialHand.ToggleAimView(false);
				break;
		}
		//Debug.Log("Chosenaction: " + inputHandler.ChosenAction);
		PlayerManager.Instance.ActionDone(gameObject);
	}
	//Cancel action
	public override void CancelAction()
	{
		inputHandler.ResetAction();

		//switch (inputHandler.ChosenAction)
		//{
		//	case TypeOfAction.ATTACK:
		//	case TypeOfAction.BOMBARD:
		//		weaponHand.ToggleAimView(false);
		//		weaponHand.CancelAction();
		//		break;
		//	case TypeOfAction.SPECIALATTACK:
		//	case TypeOfAction.SPECIALBOMBARD:
		//		specialHand.ToggleAimView(false);
		//		specialHand.CancelAction();
		//		break;
		//	case TypeOfAction.THROW:
		//		weaponHand.CancelAction();
		//		break;
		//	case TypeOfAction.REVIVE:
		//		inputHandler.PlayerToRevive = null;
		//		break;
		//}
		//Debug.LogWarning("Reset action");
	}

	//Update
	public override void OnFixedUpdateState()
	{
		if(!inputHandler.IsInputTriggered && playerMovement.MoveDirection != Vector3.zero)
		{
			inputHandler.Attributes.Stamina -= Time.deltaTime;
		}
	}

	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatTurnState" + this);
		inputHandler.Attributes.Stamina = inputHandler.Attributes.StartStamina;
		specialHand.StartTurnEffect();		
		inputHandler.ResetInput();
	}

	public override void OnStateExit()
	{
		if (inputHandler.IsInputTriggered && !inputHandler.IsInputLocked)
		{
			LockAction();			
		}
		inputHandler.LockInput();
		inputHandler.Attributes.Stamina = inputHandler.Attributes.StartStamina;
		Debug.Log("Exits CombatTurnState" + this);
	}
}
