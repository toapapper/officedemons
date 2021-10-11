using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class OutOfCombatState : AbstractPlayerState
{
    //Attack action
	public override void OnAttack()
	{
        weaponHand.Attack();
    }

    //Bombard action
    public override bool OnStartBombard()
    {
		if (weaponHand.StartBombard())
		{
            weaponHand.ToggleAimView(true);
            IsActionTriggered = true;
            return true;
        }
        return false;
    }
    public override bool OnBombard()
    {
		if (weaponHand.PerformBombard())
		{
            IsActionTriggered = false;
            weaponHand.ToggleAimView(false);
            return true;
        }
        return false;
    }

    //Special action
    public override void OnSpecial()
    {
        //TODO
        //specialHand.Attack();
    }

    //PickUp
    public override void OnPickUp(GameObject weapon)
	{
        weaponHand.Equip(weapon);
	}
	public override bool OnStartThrow()
	{
        if (weaponHand.StartThrow())
        {
            IsActionTriggered = true;

            return true;
        }
        return false;
    }
	public override bool OnThrow()
	{
		if (weaponHand.Throw())
		{
			IsActionTriggered = false;
			return true;
		}
		return false;
    }

    //Revive action
	public override void OnRevive(GameObject player)
	{
        player.GetComponentInChildren<Attributes>().Health = 100;
        Debug.Log("Revive player " + player.name);
    }

    public override void OnFixedUpdateState()
    {
        //Rotation
        if (playerMovement.CalculateRotation() != transform.rotation)
        {
            playerMovement.PerformRotation();
        }
		if (!IsActionTriggered)
		{
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
        //if (IsAddingThrowForce)
        //{
        //    weaponHand.CancelAction();
        //    playerMovement.CancelThrow();
        //    IsAddingThrowForce = false;
        //}
        Debug.Log("Exits OutOfCombatState" + this);
    }
}
