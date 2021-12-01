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

// Last Edited: 2021-10-29
public class OutOfCombatState : AbstractPlayerState
{
	//Attack action
	public override void OnAttack()
	{
		weaponHand.Attack();
		inputHandler.ResetInput();
		//if (!IsActionTriggered)
		//{
		//	weaponHand.Attack();
		//	IsActionTriggered = true;
		//}
	}

	//Bombard action
	public override void OnStartBombard()
	{
		weaponHand.ToggleAimView(true);
		weaponHand.StartBombard();

		//if (!IsActionTriggered)
		//{
		//	if (weaponHand.StartBombard())
		//	{
		//		ChosenAction = TypeOfAction.BOMBARD;
		//		weaponHand.ToggleAimView(true);
		//		IsAddingBombardForce = true;
		//		IsActionTriggered = true;
		//		return true;
		//	}
		//}
		//return false;
	}
	public override void OnBombard()
	{
		weaponHand.PerformBombard();
		weaponHand.ToggleAimView(false);
		inputHandler.ResetInput();

		//if (IsActionTriggered)
		//{
		//	if (weaponHand.PerformBombard())
		//	{
		//		ChosenAction = TypeOfAction.NOACTION;
		//		IsAddingBombardForce = false;
		//		weaponHand.ToggleAimView(false);
		//		return true;
		//	}
		//}
		//return false;
	}

	//Special action
	public override void OnSpecial()
	{
		specialHand.Attack();
		inputHandler.ResetInput();

		//if (!IsActionTriggered)
		//{
		//	specialHand.Attack();
		//	IsActionTriggered = true;
		//}
	}
	//Special Bombard action
	public override void OnStartSpecialBombard()
	{
		specialHand.StartAttack();
		specialHand.ToggleAimView(true);

		//if (!IsActionTriggered)
		//{
		//	if (specialHand.StartAttack())
		//	{
		//		Debug.Log("START SPECIAL");
		//		ChosenAction = TypeOfAction.SPECIALBOMBARD;
		//		specialHand.ToggleAimView(true);
		//		IsAddingBombardForce = true;
		//		IsActionTriggered = true;
		//		return true;
		//	}
		//}
		//return false;
	}
	public override void OnSpecialBombard()
	{
		specialHand.Attack();
		specialHand.ToggleAimView(false);
		inputHandler.ResetInput();

		//if (IsActionTriggered)
		//{
		//	if (specialHand.Attack())
		//	{
		//		Debug.Log("DO SPECIAL");
		//		ChosenAction = TypeOfAction.NOACTION;
		//		IsAddingBombardForce = false;
		//		specialHand.ToggleAimView(false);
		//		return true;
		//	}
		//}
		//return false;
	}
	public override void LockAction()
	{
		switch (inputHandler.ChosenAction)
		{
			case TypeOfAction.BOMBARD:
				OnBombard();
				break;
			case TypeOfAction.SPECIALBOMBARD:
				OnSpecialBombard();
				break;
		}
		//ChosenAction = TypeOfAction.NOACTION;
		//IsActionTriggered = false;
	}
	public override void CancelAction()
	{
		switch (inputHandler.ChosenAction)
		{
			case TypeOfAction.BOMBARD:
				weaponHand.CancelAction();
				weaponHand.ToggleAimView(false);

				break;
			case TypeOfAction.SPECIALBOMBARD:
				specialHand.CancelAction();
				specialHand.ToggleAimView(false);
				break;
		}
		//ChosenAction = TypeOfAction.NOACTION;
		//IsAddingBombardForce = false;
		//IsActionTriggered = false;
	}

	//PickUp
	public override void OnPickUp(GameObject weapon)
	{
		weaponHand.Equip(weapon);
		inputHandler.ResetInput();
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
		weaponHand.Throw();
		inputHandler.ResetInput();

		//if (IsActionTriggered)
		//{
		//	weaponHand.Throw();
		//	//IsActionTriggered = false;
		//	return true;
		//}
		//return false;
	}

	//Revive action
	public override void OnRevive(GameObject player)
	{
		Effects.Revive(player);
		inputHandler.ResetInput();

		//if (!IsActionTriggered)
		//{
		//	//player.GetComponentInChildren<Attributes>().Health = 100;
		//	Effects.Revive(player);
		//}
	}

	//Update
	//public override void OnFixedUpdateState()
	//{
	//	//Rotation
	//	if (!IsAddingBombardForce)
	//	{
	//		if (playerMovement.CalculateRotation() != transform.rotation)
	//		{
	//			playerMovement.PerformRotation();
	//		}
	//		if (!IsActionTriggered)
	//		{
	//			//Movement
	//			if (playerMovement.CalculateMovement() != Vector3.zero)
	//			{
	//				playerMovement.PerformMovement();
	//			}
	//		}
	//	}
		







	//	////Rotation
	//	//if (playerMovement.CalculateRotation() != transform.rotation)
	//	//{
	//	//	if (!IsAddingBombardForce)
	//	//	{
	//	//		playerMovement.PerformRotation();
	//	//	}
	//	//}
	//	//if (!IsActionTriggered)
	//	//{
	//	//	//Movement
	//	//	if (playerMovement.CalculateMovement() != Vector3.zero)
	//	//	{
	//	//		playerMovement.PerformMovement();
	//	//	}
	//	//}
	//}

	public override void OnStateEnter()
	{
		if (inputHandler.IsInputTriggered)
		{
			inputHandler.ResetInput();
		}



		//ChosenAction = TypeOfAction.NOACTION;
		//playerMovement.MoveDirection = Vector3.zero;
		//playerMovement.MoveAmount = Vector3.zero;

		//if (IsActionTriggered)
		//{
		//	weaponHand.CancelAction();
		//	specialHand.CancelAction();
		//	IsAddingBombardForce = false;
		//	IsActionTriggered = false;
		//}
	}

	public override void OnStateExit()
	{
		if (inputHandler.IsInputTriggered)
		{
			inputHandler.ResetInput();
		}




		//ChosenAction = TypeOfAction.NOACTION;
		//weaponHand.ToggleAimView(false);
		//specialHand.ToggleAimView(false);
		//weaponHand.CancelAction();
		//specialHand.CancelAction();
		//IsAddingBombardForce = false;
		//IsActionTriggered = false;
	}
}
