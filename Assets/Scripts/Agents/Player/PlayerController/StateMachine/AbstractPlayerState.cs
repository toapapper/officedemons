using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// <para>
/// Parent to player states
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-12
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
    private bool isAddingBombardForce;
    private TypeOfAction chosenAction;
    private bool isStaminaDepleted;

    private GameObject playerToRevive;

    protected TypeOfAction TypeOfActions
	{
        get { return typeOfActions; }
		set { typeOfActions = value; }
	}
    public TypeOfAction ChosenAction
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
    protected bool IsAddingBombardForce
	{
        get { return isAddingBombardForce; }
        set { isAddingBombardForce = value; }
    }
    public bool IsStaminaDepleted
    {
        get { return attributes.Stamina <= 0; }
    }
    public GameObject PlayerToRevive
	{
        get { return playerToRevive; }
        set { playerToRevive = value; }
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
    public virtual void LockAction() { }
    public virtual void CancelAction() { }
	public virtual void OnAttack() { }
    public virtual bool OnStartBombard() { return false; }
    public virtual bool OnBombard() { return false; }
    public virtual void OnSpecial() { }
    public virtual void OnPickUp(GameObject weapon) { }
    public virtual bool OnStartThrow() { return false; }
    public virtual bool OnThrow() { return false; }
    public virtual void OnRevive(GameObject player) { }
    public virtual void TransitionState(IPlayerState state)
    {
        OnStateExit();
        GetComponent<PlayerStateController>().CurrentState = state;
        state.OnStateEnter();
    }

	public virtual void OnFixedUpdateState() { }

    public abstract void OnStateExit();
    public abstract void OnStateEnter();
}
