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

// Last Edited: 2021-10-29
public abstract class AbstractPlayerState : MonoBehaviour, IPlayerState
{
    protected PlayerInputHandler inputHandler;
    protected PlayerMovementController playerMovement;
    protected WeaponHand weaponHand;
    protected SpecialHand specialHand;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
		playerMovement = GetComponent<PlayerMovementController>();
        weaponHand = GetComponent<WeaponHand>();
        specialHand = GetComponent<SpecialHand>();
    }

    public virtual void LockAction() { }
    public virtual void CancelAction() { }
	public virtual void OnAttack() { }
    public virtual void OnStartBombard() { }
    public virtual void OnBombard() { }
    public virtual void OnSpecial() { }
    public virtual void OnStartSpecialBombard() { }
    public virtual void OnSpecialBombard() { }
    public virtual void OnPickUp(GameObject weapon) { }
    public virtual void OnStartThrow() { }
    public virtual void OnThrow() { }
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
