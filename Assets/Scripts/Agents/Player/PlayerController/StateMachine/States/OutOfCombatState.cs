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
	}

	//Bombard action
	public override void OnStartBombard()
	{
		weaponHand.ToggleAimView(true);
		weaponHand.StartBombard();
	}
	public override void OnBombard()
	{
		weaponHand.PerformBombard();
		weaponHand.ToggleAimView(false);
		inputHandler.ResetInput();
	}

	//Special action
	public override void OnSpecial()
	{
		specialHand.Attack();
		inputHandler.ResetInput();
	}
	//Special Bombard action
	public override void OnStartSpecialBombard()
	{
		specialHand.StartAttack();
		specialHand.ToggleAimView(true);
	}
	public override void OnSpecialBombard()
	{
		specialHand.Attack();
		specialHand.ToggleAimView(false);
		inputHandler.ResetInput();
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
	}
	public override void OnThrow()
	{
		weaponHand.Throw();
		inputHandler.ResetInput();
	}

	//Revive action
	public override void OnRevive(GameObject player)
	{
		Effects.Revive(player);
		inputHandler.ResetInput();
	}

	public override void OnStateEnter()
	{
		if (inputHandler.IsInputTriggered)
		{
			inputHandler.ResetInput();
		}
	}

	public override void OnStateExit()
	{
		if (inputHandler.IsInputTriggered)
		{
			inputHandler.ResetInput();
		}
	}
}
