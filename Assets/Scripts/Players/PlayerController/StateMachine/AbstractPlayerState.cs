using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public abstract class AbstractPlayerState : MonoBehaviour, IPlayerState
{
	protected PlayerMovementController playerMovement;
    private Array typeOfActions;

    private bool isActionTriggered;
    private bool isActionLocked;
    private bool isThrowing;
    private bool isAddingThrowForce;
    private Enum chosenAction;
    private bool isStaminaDepleted;
    protected Attributes attributes;//ossian o jonas

    protected Array TypeOfActions
	{
        get { return typeOfActions; }
        set { typeOfActions = value; }
    }
    protected Enum ChosenAction
	{
        get { return chosenAction; }
        set { chosenAction = value; }
    }
    protected bool IsActionTriggered
	{
        get { return isActionTriggered; }
        set { isActionTriggered = value; }
    }
    protected bool IsActionLocked
    {
        get { return isActionLocked; }
        set { isActionLocked = value; }
    }
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

    protected bool IsStaminaDepleted
    {
        get { return attributes.Stamina <= 0; }
    }

    private void Awake()
    {
        typeOfActions = Enum.GetValues(typeof(TypeOfAction));
        playerMovement = GetComponent<PlayerMovementController>();

        attributes = GetComponent<Attributes>();//j+o
    }

    public abstract void OnMove(CallbackContext context);
    public abstract void OnAttack(CallbackContext context);
    public abstract void OnSpecial(CallbackContext context);
    public abstract void OnPickupThrow(CallbackContext context);
    public abstract void OnRevive(CallbackContext context);

	public abstract void OnFixedUpdateState();

	public abstract void OnStateExit();
    public abstract void OnStateEnter();
}
