using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class OutOfCombatState : AbstractPlayerState
{
    //Move
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
        throw new System.NotImplementedException();
    }
    public override void OnThrow(CallbackContext context)
    {
        if (context.started)
        {
            IsThrowing = playerMovement.StartThrow();
        }
        else if (context.canceled && IsThrowing)
        {
            playerMovement.PerformThrow(AddedThrowForce);
            IsThrowing = false;
        }

        if (IsWeaponEquipped)
        {
            if (context.started)
            {
                IsThrowing = true;
                WeaponHand.AimThrow();
            }
            else if (context.canceled)
            {
                WeaponHand.Throw(AddedThrowForce);
                IsWeaponEquipped = false;
                IsThrowing = false;
            }
        }
        throw new System.NotImplementedException();
    }
	public override void OnPickup(CallbackContext context)
	{
        playerMovement.PerformPickup();
    }


    private void FixedUpdate()
    {
        //Rotation
        if (transform.rotation != playerMovement.CalculateRotation())
        {
            playerMovement.PerformRotation();
        }
        if (IsThrowing)
        {
            //Throwing
            if (isAddingThrowForce && addedThrowForce <= maxThrowForce)
            {
                addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
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
        if (transform.position.y > 0)
        {
            playerMovement.PerformFall();
        }
    }




    public override void OnStateEnter()
    {
        Debug.Log("Enters OutOfCombatState");
    }

    public override void OnStateExit()
    {
        Debug.Log("Exits OutOfCombatState");
    }
}
