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

// Last Edited: 2021-10-21

public class CombatTurnState : AbstractPlayerState
{
	//Attack Action
	public override void OnAttack()
	{
		if (!IsActionTriggered)
		{
			weaponHand.ToggleAimView(true);
			weaponHand.StartAttack();
			ChosenAction = TypeOfAction.ATTACK;
			IsActionTriggered = true;
		}
	}

	//Bombard action
	public override bool OnStartBombard()
	{
		if (!IsActionTriggered)
		{
			if (weaponHand.StartBombard())
			{
				weaponHand.ToggleAimView(true);
				ChosenAction = TypeOfAction.BOMBARD;
				IsActionTriggered = true;
				return true;
			}
		}
		return false;
	}
	public override bool OnBombard()
	{
		if (IsActionTriggered)
		{
			return true;
		}
		return false;
	}

	//Special Action
	public override void OnSpecial()
	{
		//TODO
		if (!IsActionTriggered)
		{
			specialHand.ToggleAimView(true);
			specialHand.StartAttack();
			ChosenAction = TypeOfAction.SPECIALATTACK;
			IsActionTriggered = true;
		}
	}

	//PickUp action
	public override void OnPickUp(GameObject weapon)
	{
		if (!IsActionTriggered)
		{
			weaponHand.Equip(weapon);
		}
	}

	//Throw action
	public override bool OnStartThrow()
	{
		if (!IsActionTriggered)
		{
			if (weaponHand.StartThrow())
			{
				//weaponHand.ToggleThrowAimView(true);
				ChosenAction = TypeOfAction.THROW;
				IsActionTriggered = true;
				return true;
			}
		}
		return false;
	}
	public override bool OnThrow()
	{
		if (IsActionTriggered)
		{
			return true;
		}
		return false;
	}

	//Revive action
	public override void OnRevive(GameObject player)
	{
		if (!IsActionTriggered)
		{
			PlayerToRevive = player;
			ChosenAction = TypeOfAction.REVIVE;
			IsActionTriggered = true;
		}
	}

	//Lock action
	public override void LockAction()
	{
		if (IsActionTriggered)
		{
			switch (ChosenAction)
			{
				case TypeOfAction.ATTACK:
					weaponHand.ToggleAimView(false);
					break;
				case TypeOfAction.BOMBARD:
					weaponHand.ToggleAimView(false);
					break;
				case TypeOfAction.SPECIALATTACK:
					//TODO
					specialHand.ToggleAimView(false);
					break;
				case TypeOfAction.THROW:
					//TODO
					//weaponHand.ToggleThrowAimView(false);
					break;
			}
			IsActionLocked = true;
			Debug.Log("Chosenaction: " + ChosenAction);
			PlayerManager.Instance.ActionDone(gameObject);
		}
	}

	//Cancel action
	public override void CancelAction()
	{
		if (IsActionTriggered)
		{
			switch (ChosenAction)
			{
				case TypeOfAction.ATTACK:
					weaponHand.ToggleAimView(false);
					weaponHand.CancelAction();
					break;
				case TypeOfAction.BOMBARD:
					weaponHand.ToggleAimView(false);
					weaponHand.CancelAction();
					break;
				case TypeOfAction.SPECIALATTACK:
					//TODO
					specialHand.ToggleAimView(false);
					specialHand.CancelAction();
					break;
				case TypeOfAction.THROW:
					//TODO
					weaponHand.CancelAction();
					//weaponHand.ToggleThrowAimView(false);
					break;
				case TypeOfAction.REVIVE:
					PlayerToRevive = null;
					break;
			}
			Debug.LogWarning("Reset action");
			ChosenAction = TypeOfAction.NOACTION;
			IsActionTriggered = false;
		}
	}

	//Update
	public override void OnFixedUpdateState()
	{
		//Rotation
		if (playerMovement.CalculateRotation() != transform.rotation)
		{
			playerMovement.PerformRotation();
		}
		if (!IsActionTriggered && !IsStaminaDepleted)
		{
			//Movement
			if (playerMovement.CalculateMovement() != Vector3.zero)
			{
				playerMovement.PerformMovement();

				if (GetComponent<PlayerMovementController>().MoveDirection != Vector3.zero)
					attributes.Stamina -= Time.deltaTime;
			}
		}
	}

	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatTurnState" + this);
		attributes.Stamina = attributes.StartStamina;
		playerMovement.MoveDirection = Vector3.zero;
		playerMovement.MoveAmount = Vector3.zero;
	}

	public override void OnStateExit()
	{
		weaponHand.ToggleAimView(false);

		if (IsActionTriggered && !IsActionLocked)
		{
			PlayerManager.Instance.ActionDone(gameObject);
		}

		IsActionLocked = false;
		IsActionTriggered = false;
		IsAddingThrowForce = false;
		Debug.Log("Exits CombatTurnState" + this);
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
}
