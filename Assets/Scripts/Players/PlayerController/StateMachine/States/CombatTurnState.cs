using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class CombatTurnState : AbstractPlayerState
{

    public override void OnMove(CallbackContext context)
	{
		Debug.Log("CombatTurnState.OnMove  " + attributes.gameObject.ToString());
		if(attributes.Stamina > 0)
        {
			playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
			attributes.Stamina -= Time.deltaTime;
        }
	}

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
	public override void OnSpecial(CallbackContext context)
	{
		//TODO
		if (context.performed && !IsActionLocked)
		{
			if (!IsActionTriggered)
			{
				playerMovement.StartSpecialAttack();
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
						playerMovement.CancelSpecialAttack();
						break;
					case TypeOfAction.THROW:
						playerMovement.CancelThrow();
						break;
					case TypeOfAction.HEAL:
						//playerMovement.CancelHeal();
						break;
					case TypeOfAction.PICKUP:
						//playerMovement.CancelPickup();
						break;
					case TypeOfAction.NOACTION:
						break;
				}
				IsActionTriggered = false;
			}
		}
	}

	//public override void OnThrow(CallbackContext context)
	//{
	//	if (context.started && !IsActionTriggered && !IsActionLocked)
	//	{
	//		if (playerMovement.StartThrow())
	//		{
	//			ChosenAction = TypeOfAction.THROW;
	//			IsActionTriggered = true;
	//			IsAddingThrowForce = true;
	//		}
	//	}
	//	else if (context.canceled && IsActionTriggered && !IsActionLocked)
	//	{
	//		if (IsAddingThrowForce)
	//		{
	//			IsAddingThrowForce = false;
	//		}
	//	}
	//}


	public override void OnPickupThrow(CallbackContext context)
	{
		if(!IsActionLocked)
		{
			if (playerMovement.isWeaponEquipped)
			{
				if (!IsActionTriggered && context.performed)
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
	public override void OnHeal(CallbackContext context)
	{
		if (context.performed && !IsActionTriggered && !IsActionLocked)
		{
			if (playerMovement.StartHeal())
			{
				ChosenAction = TypeOfAction.HEAL;
				IsActionTriggered = true;
			}
		}
	}
	public override void OnRevive(CallbackContext context)
	{
		if (context.performed && !IsActionTriggered && !IsActionLocked)
		{
			if (playerMovement.StartRevive())
			{
				ChosenAction = TypeOfAction.HEAL;
				IsActionTriggered = true;
			}
		}
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
		if (IsActionTriggered)
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
