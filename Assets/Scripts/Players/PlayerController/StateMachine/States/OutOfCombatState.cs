using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class OutOfCombatState : AbstractPlayerState
{
    public override void OnMove(CallbackContext context)
	{
		playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
    }

    public override void OnAttack(CallbackContext context)
	{
		if (context.performed)
		{
            playerMovement.PerformAttack();
        }
    }
    public override void OnSpecial(CallbackContext context)
    {
        //TODO
        //if (context.performed)
        //{
        //    SpecialWeaponHand.Attack();
        //}
    }
    public override void OnThrow(CallbackContext context)
    {
        if (context.started && !IsThrowing)
        {
			if (playerMovement.StartThrow())
			{
                IsThrowing = true;
                IsAddingThrowForce = true;
			}
        }
        else if (context.canceled && IsThrowing)
        {
			if (playerMovement.PerformThrow())
			{
                IsThrowing = false;
                IsAddingThrowForce = true;
            }
        }
    }
	public override void OnPickup(CallbackContext context)
	{
		if (context.performed)
        {
            playerMovement.PerformPickup();
        }
    }
	public override void OnRevive(CallbackContext context)
	{
		if (context.performed)
		{
            throw new System.NotImplementedException();
        }
	}

	public override void OnFixedUpdateState()
    {
        //Rotation
        if (playerMovement.CalculateRotation() != transform.rotation)
        {
            playerMovement.PerformRotation();
        }
        //Throwing
        if (IsThrowing && IsAddingThrowForce)
        {
            playerMovement.AddThrowForce();
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
        Debug.Log("Enters OutOfCombatState" + this);
    }

    public override void OnStateExit()
    {
        Debug.Log("Exits OutOfCombatState" + this);
    }
}
