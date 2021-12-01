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

	public override void OnRevive(GameObject player)
	{
		PlayerToRevive = player;
	}

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
		Debug.Log("Chosenaction: " + inputHandler.ChosenAction);
		PlayerManager.Instance.ActionDone(gameObject);
	}

	public override void CancelAction()
	{
		switch (inputHandler.ChosenAction)
		{
			case TypeOfAction.ATTACK:
			case TypeOfAction.BOMBARD:
				weaponHand.ToggleAimView(false);
				weaponHand.CancelAction();
				break;
			case TypeOfAction.SPECIALATTACK:
			case TypeOfAction.SPECIALBOMBARD:
				specialHand.ToggleAimView(false);
				specialHand.CancelAction();
				break;
			case TypeOfAction.THROW:
				weaponHand.CancelAction();
				break;
			case TypeOfAction.REVIVE:
				PlayerToRevive = null;
				break;
		}
		Debug.LogWarning("Reset action");
	}

	//Update
	public override void OnFixedUpdateState()
	{
		if(playerMovement.MoveDirection != Vector3.zero)
		{
			attributes.Stamina -= Time.deltaTime;
		}
	}

	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatTurnState" + this);
		attributes.Stamina = attributes.StartStamina;
		specialHand.StartTurnEffect();		
		inputHandler.ResetInput();
	}

	public override void OnStateExit()
	{
		if (inputHandler.IsInputTriggered && !inputHandler.IsInputLocked)
		{
			LockAction();			
			//PlayerManager.Instance.ActionDone(gameObject);
		}
		inputHandler.LockInput();
		Debug.Log("Exits CombatTurnState" + this);
	}
}
