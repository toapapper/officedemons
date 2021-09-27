using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public abstract class AbstractPlayerState : MonoBehaviour, IPlayerState
{
	protected PlayerMovementController playerMovement;

    private bool isThrowing;
    private bool isAddingThrowForce;

    protected bool IsThrowing
    {
        get { return isThrowing; }
        set { isThrowing = value; }
    }
    protected bool IsAddingThrowForce
    {
        get { return isAddingThrowForce; }
        set { isAddingThrowForce = value; }
    }


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovementController>();
    }

    public abstract void OnMove(CallbackContext context);
    public abstract void OnAttack(CallbackContext context);
    public abstract void OnSpecial(CallbackContext context);
    public abstract void OnThrow(CallbackContext context);
    public abstract void OnPickup(CallbackContext context);
    public abstract void OnRevive(CallbackContext context);

	public abstract void OnFixedUpdateState();

	public abstract void OnStateExit();
    public abstract void OnStateEnter();
}
