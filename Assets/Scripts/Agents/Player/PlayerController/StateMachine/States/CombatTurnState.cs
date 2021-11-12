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
		if (!IsActionTriggered)
		{
			weaponHand.ToggleAimView(true);
			weaponHand.StartAttack();
			ChosenAction = TypeOfAction.ATTACK;
			IsActionTriggered = true;
		}
	}

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

	public override void OnSpecial()
	{
		if (!IsActionTriggered)
		{
			specialHand.ToggleAimView(true);
			specialHand.StartAttack();
			ChosenAction = TypeOfAction.SPECIALATTACK;
			IsActionTriggered = true;
		}
	}
	public override bool OnStartSpecialBombard()
	{
		if (!IsActionTriggered)
		{
			if (specialHand.StartAttack())
			{
				specialHand.ToggleAimView(true);
				ChosenAction = TypeOfAction.SPECIALBOMBARD;
				IsActionTriggered = true;
				return true;
			}
		}
		return false;
	}
	public override bool OnSpecialBombard()
	{
		if (IsActionTriggered)
		{
			return true;
		}
		return false;
	}

	public override void OnPickUp(GameObject weapon)
	{
		if (!IsActionTriggered)
		{
			weaponHand.Equip(weapon);
		}
	}

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
				case TypeOfAction.BOMBARD:
					weaponHand.ToggleAimView(false);
					break;
				case TypeOfAction.SPECIALATTACK:
				case TypeOfAction.SPECIALBOMBARD:
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

	public override void CancelAction()
	{
		if (IsActionTriggered)
		{
			switch (ChosenAction)
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
					//TODO
					//weaponHand.ToggleThrowAimView(false);
					weaponHand.CancelAction();
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
		specialHand.StartTurnEffect();
		attributes.Stamina = attributes.StartStamina;
		playerMovement.MoveDirection = Vector3.zero;
		playerMovement.MoveAmount = Vector3.zero;
	}

	public override void OnStateExit()
	{
		weaponHand.ToggleAimView(false);
		specialHand.ToggleAimView(false);

		if (IsActionTriggered && !IsActionLocked)
		{
			PlayerManager.Instance.ActionDone(gameObject);
		}

		IsActionLocked = false;
		IsActionTriggered = false;
		IsAddingThrowForce = false;
		Debug.Log("Exits CombatTurnState" + this);
	}
}
