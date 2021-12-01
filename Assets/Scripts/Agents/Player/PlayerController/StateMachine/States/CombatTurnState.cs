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

		//if (!IsActionTriggered)
		//{
		//	weaponHand.ToggleAimView(true);
		//	weaponHand.StartAttack();
		//	ChosenAction = TypeOfAction.ATTACK;
		//	IsActionTriggered = true;
		//}
	}

	public override void OnStartBombard()
	{
		weaponHand.ToggleAimView(true);
		weaponHand.StartBombard();

		//if (!IsActionTriggered)
		//{
		//	if (weaponHand.StartBombard())
		//	{
		//		weaponHand.ToggleAimView(true);
		//		ChosenAction = TypeOfAction.BOMBARD;
		//		IsActionTriggered = true;
		//		return true;
		//	}
		//}
		//return false;
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

		//if (!IsActionTriggered)
		//{
		//	specialHand.ToggleAimView(true);
		//	specialHand.StartAttack();
		//	ChosenAction = TypeOfAction.SPECIALATTACK;
		//	IsActionTriggered = true;
		//}
	}
	public override void OnStartSpecialBombard()
	{
		specialHand.ToggleAimView(true);
		specialHand.StartAttack();

		//if (!IsActionTriggered)
		//{
		//	if (specialHand.StartAttack())
		//	{
		//		specialHand.ToggleAimView(true);
		//		ChosenAction = TypeOfAction.SPECIALBOMBARD;
		//		IsActionTriggered = true;
		//		return true;
		//	}
		//}
		//return false;
	}

	public override void OnPickUp(GameObject weapon)
	{
		weaponHand.Equip(weapon);

		//if (!IsActionTriggered)
		//{
		//	weaponHand.Equip(weapon);
		//}
	}

	public override void OnStartThrow()
	{
		weaponHand.StartThrow();

		//if (!IsActionTriggered)
		//{
		//	weaponHand.StartThrow();
		//	IsActionTriggered = true;
		//	return true;
		//}
		//return false;
	}
	public override void OnThrow()
	{
		LockAction();

		//if (IsActionTriggered)
		//{
		//	ChosenAction = TypeOfAction.THROW;
		//	LockAction();
		//	//IsActionTriggered = false;
		//	return true;
		//}
		//return false;
	}

	public override void OnRevive(GameObject player)
	{
		PlayerToRevive = player;

		//if (!IsActionTriggered)
		//{
		//	PlayerToRevive = player;
		//	ChosenAction = TypeOfAction.REVIVE;
		//	IsActionTriggered = true;
		//}
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



		//if (IsActionTriggered)
		//{
		//	switch (ChosenAction)
		//	{
		//		case TypeOfAction.ATTACK:
		//		case TypeOfAction.BOMBARD:
		//			weaponHand.ToggleAimView(false);
		//			break;
		//		case TypeOfAction.SPECIALATTACK:
		//		case TypeOfAction.SPECIALBOMBARD:
		//			specialHand.ToggleAimView(false);
		//			break;
		//	}
		//	IsActionLocked = true;
		//	Debug.Log("Chosenaction: " + ChosenAction);
		//	PlayerManager.Instance.ActionDone(gameObject);
		//}
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




		//if (IsActionTriggered)
		//{
		//	switch (ChosenAction)
		//	{
		//		case TypeOfAction.ATTACK:
		//		case TypeOfAction.BOMBARD:
		//			weaponHand.ToggleAimView(false);
		//			weaponHand.CancelAction();
		//			break;
		//		case TypeOfAction.SPECIALATTACK:
		//		case TypeOfAction.SPECIALBOMBARD:
		//			specialHand.ToggleAimView(false);
		//			specialHand.CancelAction();
		//			break;
		//		case TypeOfAction.THROW:
		//			weaponHand.CancelAction();
		//			break;
		//		case TypeOfAction.REVIVE:
		//			PlayerToRevive = null;
		//			break;
		//	}
		//	Debug.LogWarning("Reset action");
		//	ChosenAction = TypeOfAction.NOACTION;
		//	IsActionTriggered = false;
		//}
	}

	//Update
	public override void OnFixedUpdateState()
	{
		if(playerMovement.MoveDirection != Vector3.zero)
		{
			attributes.Stamina -= Time.deltaTime;
		}





		//if (!IsAddingBombardForce)
		//{
		//	//Rotation
		//	if (playerMovement.CalculateRotation() != transform.rotation)
		//	{
		//		playerMovement.PerformRotation();
		//	}
		//	if (!IsActionTriggered && !IsStaminaDepleted)
		//	{
		//		//Movement
		//		if (playerMovement.CalculateMovement() != Vector3.zero)
		//		{
		//			playerMovement.PerformMovement();

		//			if (GetComponent<PlayerMovementController>().MoveDirection != Vector3.zero)
		//				attributes.Stamina -= Time.deltaTime;
		//		}
		//	}
		//}
	}

	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatTurnState" + this);
		specialHand.StartTurnEffect();
		attributes.Stamina = attributes.StartStamina;
		playerMovement.MoveDirection = Vector3.zero;
		playerMovement.MoveAmount = Vector3.zero;

		if (IsActionTriggered)
		{
			weaponHand.CancelAction();
			specialHand.CancelAction();
			IsAddingBombardForce = false;
			IsActionTriggered = false;
		}
	}

	public override void OnStateExit()
	{
		//weaponHand.ToggleAimView(false);
		//specialHand.ToggleAimView(false);
		//specialHand.EndTurnEffects();

		if (inputHandler.IsInputTriggered && !inputHandler.IsInputLocked)
		{
			LockAction();
			//PlayerManager.Instance.ActionDone(gameObject);
		}
		Debug.Log("Exits CombatTurnState" + this);








		//weaponHand.ToggleAimView(false);
		//specialHand.ToggleAimView(false);
		//specialHand.EndTurnEffects();
		//if (IsActionTriggered && !IsActionLocked)
		//{
		//	PlayerManager.Instance.ActionDone(gameObject);
		//}

		//IsActionLocked = false;
		//IsActionTriggered = false;
		//IsAddingThrowForce = false;
		//IsAddingBombardForce = false;
		//Debug.Log("Exits CombatTurnState" + this);
	}
}
