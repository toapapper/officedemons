using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class CombatTurnState : AbstractPlayerState
{
	//Move action
	public override void OnMove(CallbackContext context)
	{
		playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
		Debug.Log("CombatTurnState.OnMove  " + attributes.gameObject.ToString());

		
	}
	//Attack Action
	public override void OnAttack(CallbackContext context)
	{
		if (context.performed && !IsActionLocked)
		{
			if (!IsActionTriggered)
			{
				playerMovement.StartAttack();
				ChosenAction = TypeOfAction.ATTACK;
				IsActionTriggered = true;
			}
			else
			{
				IsActionTriggered = false;
				IsActionLocked = true;
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
				IsActionTriggered = false;
			}
		}
	}
	//Pickup/Throw action
	public override void OnPickupThrow(CallbackContext context)
	{
		if(!IsActionLocked)
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
		attributes.Stamina = attributes.StartStamina;
		Debug.Log("Enters CombatTurnState" + this);
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits CombatTurnState" + this);
	}
}
