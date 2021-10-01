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
    protected WeaponHand weaponHand;
    protected Attributes attributes;//ossian o jonas
    protected CharacterController characterController;

    private TypeOfAction typeOfActions;

    private bool isActionTriggered;
    private bool isActionLocked;
    private bool isAddingThrowForce;
    private static Enum chosenAction;
    private bool isStaminaDepleted;

    protected TypeOfAction TypeOfActions
	{
        get { return typeOfActions; }
		set { typeOfActions = value; }
	}
    protected Enum ChosenAction
	{
        get { return chosenAction; }
        set { chosenAction = value; }
    }
    public bool IsActionTriggered
	{
        get { return isActionTriggered; }
        set { isActionTriggered = value; }
    }
    public bool IsActionLocked
    {
        get { return isActionLocked; }
        set { isActionLocked = value; }
    }
    protected bool IsAddingThrowForce
    {
        get { return isAddingThrowForce; }
        set { isAddingThrowForce = value; }
    }
    public bool IsStaminaDepleted
    {
        get { return attributes.Stamina <= 0; }
    }

    private void Awake()
    {
		ChosenAction = TypeOfAction.NOACTION;
		playerMovement = GetComponent<PlayerMovementController>();
        weaponHand = GetComponent<WeaponHand>();

        attributes = GetComponent<Attributes>();//ossian o jonas
        characterController = GetComponent<CharacterController>();
    }

    //public abstract void OnMove(CallbackContext context);
    public abstract void LockAction();
    public abstract void CancelAction();
	public abstract void OnAttack();
    public abstract void OnSpecial();
    public abstract void OnPickUp(GameObject weapon);
    public abstract void OnThrow(CallbackContext context);
    public abstract void OnRevive(CallbackContext context);

	public abstract void OnFixedUpdateState();

	public abstract void OnStateExit();
    public abstract void OnStateEnter();
}
