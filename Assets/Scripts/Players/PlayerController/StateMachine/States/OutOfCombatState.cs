using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class OutOfCombatState : AbstractPlayerState
{
	//Move action
	//   public override void OnMove(CallbackContext context)
	//{
	//       playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
	//   }
	//Attack action
	public override void LockAction() { }
    public override void CancelAction() { }
	public override void OnAttack()
	{
        weaponHand.Attack();
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
    public override void OnThrow(CallbackContext context)
    {
        if (context.started)
        {
            if (weaponHand.StartThrow())
            {
                IsAddingThrowForce = true;
            }
        }
        else if (context.canceled)
        {
            if (playerMovement.PerformThrow())
            {
                IsAddingThrowForce = false;
            }
        }
    }
 //   //Pickup/Throw action
 //   public override void OnPickupThrow(CallbackContext context)
	//{
	//	if (!playerMovement.isWeaponEquipped)
	//	{
 //           if (context.canceled)
 //           {
 //               playerMovement.PerformPickup();
 //           }
 //       }
	//	else
	//	{
 //           if (context.started)
 //           {
 //               if (playerMovement.StartThrow())
 //               {
 //                   IsAddingThrowForce = true;
 //               }
 //           }
 //           else if (context.canceled)
 //           {
 //               if (playerMovement.PerformThrow())
 //               {
 //                   IsAddingThrowForce = false;
 //               }
 //           }
 //       }
 //   }
    //Revive action
	public override void OnRevive(CallbackContext context)
	{
		//if (context.performed)
		//{
  //          throw new System.NotImplementedException();
  //      }
	}
    //Heal action
    //   public override void OnHeal(CallbackContext context)
    //{
    //       if (context.performed)
    //	{
    //           playerMovement.PerformHeal();

    //       }
    //   }


    public override void OnFixedUpdateState()
    {
        //Rotation
        if (playerMovement.CalculateRotation() != transform.rotation)
        {
            playerMovement.PerformRotation();
        }
        //Throwing
        if (IsAddingThrowForce)
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
        Debug.LogWarning("Reset action");
        ChosenAction = TypeOfAction.NOACTION;
    }

    public override void OnStateExit()
    {
        if (IsAddingThrowForce)
        {
            playerMovement.CancelThrow();
            IsAddingThrowForce = false;
        }
        Debug.Log("Exits OutOfCombatState" + this);
    }
}
