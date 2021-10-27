using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Handle character actions when roaming freely out of combat
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-21
public class OutOfCombatState : AbstractPlayerState
{
	//Attack action
	public override void OnAttack()
	{
		if (!IsActionTriggered)
		{
			weaponHand.Attack();
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
			if (weaponHand.PerformBombard())
			{
				IsActionTriggered = false;
				weaponHand.ToggleAimView(false);
				return true;
			}
		}
		return false;
	}

	//Special action
	public override void OnSpecial()
	{
		//TODO
		if (!IsActionTriggered)
		{
			specialHand.Attack();
		}
	}

	//PickUp
	public override void OnPickUp(GameObject weapon)
	{
		weaponHand.Equip(weapon);
	}
	public override bool OnStartThrow()
	{
		if (!IsActionTriggered)
		{
			if (weaponHand.StartThrow())
			{
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
			if (weaponHand.Throw())
			{
				IsActionTriggered = false;
				return true;
			}
		}
		return false;
	}

	//Revive action
	public override void OnRevive(GameObject player)
	{
		if (!IsActionTriggered)
		{
			player.GetComponentInChildren<Attributes>().Health = 100;
			Debug.Log("Revive player " + player.name);
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
		if (!IsActionTriggered)
		{
			//Movement
			if (playerMovement.CalculateMovement() != Vector3.zero)
			{
				playerMovement.PerformMovement();
			}
		}
	}

	public override void OnStateEnter()
	{
		Debug.Log("Enters OutOfCombatState" + this);
		Debug.LogWarning("Reset action");
		ChosenAction = TypeOfAction.NOACTION;
		playerMovement.MoveDirection = Vector3.zero;
		playerMovement.MoveAmount = Vector3.zero;
	}

	public override void OnStateExit()
	{
		if (IsActionTriggered)
		{
			weaponHand.CancelAction();
			IsActionTriggered = false;
		}
		Debug.Log("Exits OutOfCombatState" + this);
	}
}
