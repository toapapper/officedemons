using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class CombatTurnState : AbstractPlayerState
{
	//Pickup/Throw action
	public override void OnPickupThrow(CallbackContext context)
	{
		if (!IsActionLocked)
		{
			if (!playerMovement.isWeaponEquipped)
			{
				if (!IsActionTriggered && context.canceled)
				{
					playerMovement.PerformPickup();
				}
			}
			else
			{
				if (context.started && !IsActionTriggered)
				{
					if (playerMovement.StartThrow())
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
		}
	}

	//Attack Action
	public override void OnAttack(CallbackContext context)
	{
		if (context.performed && !IsActionLocked)
		{
			if (!IsActionTriggered)
			{
				playerMovement.ToggleWeaponAimView(true);
				playerMovement.StartAttack();
				ChosenAction = TypeOfAction.ATTACK;
				IsActionTriggered = true;
			}
			else
			{
				IsActionTriggered = false;
				IsActionLocked = true;
				playerMovement.ToggleWeaponAimView(false);
				Debug.Log("Chosenaction: " + ChosenAction);
				PlayerManager.instance.ActionDone(gameObject);
			}

		}
	}
	//Special Action
	public override void OnSpecial(CallbackContext context)
	{
		//TODO
		if (context.performed && !IsActionLocked)
		{
			if (!IsActionTriggered)
			{
				playerMovement.StartSpecial();
				ChosenAction = TypeOfAction.SPECIALATTACK;
				IsActionTriggered = true;
			}
			else
			{
				switch (ChosenAction)
				{
					case TypeOfAction.ATTACK:
						playerMovement.ToggleWeaponAimView(false);
						playerMovement.CancelAttack();
						break;
					case TypeOfAction.SPECIALATTACK:
						playerMovement.CancelSpecial();
						break;
					case TypeOfAction.THROW:
						playerMovement.CancelThrow();
						break;
					case TypeOfAction.REVIVE:
						playerMovement.CancelRevive();
						break;
					case TypeOfAction.NOACTION:
						break;
				}
				Debug.LogWarning("Reset action");
				ChosenAction = TypeOfAction.NOACTION;
				IsActionTriggered = false;
			}
		}
	}

	//Revive action
	public override void OnRevive(CallbackContext context)
	{
		if (!IsActionLocked)
		{
			if(context.started && !IsActionTriggered)
			{
				IsActionTriggered = true;
			}
			else if(context.canceled && IsActionTriggered)
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
	public override void OnMove(CallbackContext context)
	{
		playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
	}

	public override void OnFixedUpdateState()
	{
		if (IsActionLocked)
			return;


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
		playerMovement.ToggleWeaponAimView(false);
   //     if (IsActionLocked || IsActionTriggered)
   //     {
			//PlayerManager.instance.ActionDone(gameObject);
   //     }
		IsActionLocked = false;
		IsActionTriggered = false;
		IsAddingThrowForce = false;
		Debug.Log("Exits CombatTurnState" + this);
	}
}
