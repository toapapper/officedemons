using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void FixedUpdateContext()
    {
        CurrentState.OnFixedUpdateState();
    }
}
