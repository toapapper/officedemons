using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class PlayerStateContext
{
    public IPlayerState CurrentState
    {
        get;
        set;
    }

    public PlayerStateContext(IPlayerState state)
    {
        CurrentState = state;
    }

    public void MakeStateTransistion(IPlayerState playerStateTo)
    {
        CurrentState.OnStateExit();
        CurrentState = playerStateTo;
        CurrentState.OnStateEnter();
    }
    public void OnMove(CallbackContext context)
    {
        CurrentState.OnMove(context);
    }
    public void OnAttack(CallbackContext context)
    {
        CurrentState.OnAttack(context);
    }
    public void OnSpecial(CallbackContext context)
    {
        CurrentState.OnSpecial(context);
    }
    public void OnPickupThrow(CallbackContext context)
    {
        CurrentState.OnPickupThrow(context);
    }
    public void OnRevive(CallbackContext context)
    {
        CurrentState.OnRevive(context);
    }

    public void FixedUpdateContext()
    {
        CurrentState.OnFixedUpdateState();
    }
}
