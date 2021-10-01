using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class CombatTurnState : AbstractPlayerState
{
	public override void LockAction()
	{
		switch (ChosenAction)
		{
			case TypeOfAction.ATTACK:
				weaponHand.ToggleAimView(false);
				break;
			case TypeOfAction.SPECIALATTACK:
				//TODO
				//playerMovement.ToggleSpecialAimView(false);
				break;
			case TypeOfAction.THROW:
				//TODO
				//playerMovement.ToggleThrowAimView(false);
				break;
		}
		IsActionLocked = true;
	}
	public override void CancelAction()
	{
		switch (ChosenAction)
		{
			case TypeOfAction.ATTACK:
				weaponHand.ToggleAimView(false);
				weaponHand.CancelAction();
				break;
			case TypeOfAction.SPECIALATTACK:
				//TODO
				//specialHand.ToggleAimView(false);
				//specialHand.CancelAction();
				break;
			case TypeOfAction.THROW:
				//TODO
				//playerMovement.ToggleThrowAimView(false);
				weaponHand.CancelAction();
				break;
			case TypeOfAction.REVIVE:
				playerMovement.CancelRevive();
				break;
		}
		ChosenAction = TypeOfAction.NOACTION;
		IsActionTriggered = false;
	}
	//PickUp
	public override void OnPickUp(GameObject weapon)
	{
		if (!IsActionTriggered)
		{
			weaponHand.Equip(weapon);
		}
	}
	//Throw
	public override void OnThrow(CallbackContext context)
	{
		if (context.started && !IsActionTriggered)
		{
			if (weaponHand.StartThrow())
			{
				ChosenAction = TypeOfAction.THROW;
				IsActionTriggered = true;
				IsAddingThrowForce = true;
			}
		}
		else if (context.canceled && IsActionTriggered)
		{
			if (IsAddingThrowForce)
			{
				IsAddingThrowForce = false;
			}
		}
	}


	//Pickup/Throw action
	//public override void OnPickupThrow(CallbackContext context)
	//{
	//	if (!playerMovement.isWeaponEquipped)
	//	{
	//		if (!IsActionTriggered && context.canceled)
	//		{
	//			playerMovement.PerformPickup();
	//		}
	//	}
	//	else
	//	{
	//		if (context.started && !IsActionTriggered)
	//		{
	//			if (playerMovement.StartThrow())
	//			{
	//				ChosenAction = TypeOfAction.THROW;
	//				IsActionTriggered = true;
	//				IsAddingThrowForce = true;
	//			}
	//		}
	//		else if (context.canceled && IsActionTriggered)
	//		{
	//			if (IsAddingThrowForce)
	//			{
	//				IsAddingThrowForce = false;
	//			}
	//		}
	//	}
	//}

	//Attack Action
	public override void OnAttack()
	{
		weaponHand.ToggleAimView(true);
		weaponHand.StartAttack();
		ChosenAction = TypeOfAction.ATTACK;
		IsActionTriggered = true;
	}
	//Special Action
	public override void OnSpecial()
	{
		//TODO
		//specialHand.ToggleAimView(true);
		//specialHand.StartAttack();
		ChosenAction = TypeOfAction.SPECIALATTACK;
		IsActionTriggered = true;

	}

	//Revive action
	public override void OnRevive(CallbackContext context)
	{
		if (context.started && !IsActionTriggered)
		{
			IsActionTriggered = true;
		}
		else if (context.canceled && IsActionTriggered)
		{
			if (playerMovement.StartRevive())
			{
				ChosenAction = TypeOfAction.REVIVE;
			}
			else
			{
				IsActionTriggered = false;
			}
		}
	}
	//Heal action
	//public override void OnHeal(CallbackContext context)
	//{
	//	if (context.performed && !IsActionTriggered && !IsActionLocked)
	//	{
	//		if (playerMovement.StartHeal())
	//		{
	//			ChosenAction = TypeOfAction.HEAL;
	//			IsActionTriggered = true;
	//		}
	//	}
	//}

	//Move action
	//public override void OnMove(CallbackContext context)
	//{
	//	playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
	//}

	public override void OnFixedUpdateState()
	{
		//if (IsActionLocked)
		//	return;


		//Rotation
		if (playerMovement.CalculateRotation() != transform.rotation)
		{
			playerMovement.PerformRotation();
		}
		if (IsActionTriggered || IsStaminaDepleted)
		{
			//Throwing
			if (IsAddingThrowForce)
			{
				playerMovement.AddThrowForce();
			}
		}
		//Movement
		else
		{
			if (playerMovement.CalculateMovement() != Vector3.zero)
			{
				attributes.Stamina -= Time.deltaTime;
				playerMovement.PerformMovement();
			}
		}
		//Falling
		if (transform.position.y > 0)
		{
			playerMovement.PerformFall();
		}
	}

	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatTurnState" + this);
		attributes.Stamina = attributes.StartStamina;
	}

	public override void OnStateExit()
	{
		weaponHand.ToggleAimView(false);
		IsActionLocked = false;
		IsActionTriggered = false;
		IsAddingThrowForce = false;
		Debug.Log("Exits CombatTurnState" + this);
	}
}
