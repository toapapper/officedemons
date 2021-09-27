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

    private readonly PlayerStateController playerStateCtrl;

    public PlayerStateContext(PlayerStateController playerStateCtrl)
    {
        this.playerStateCtrl = playerStateCtrl;
        CurrentState = new OutOfCombatState();
    }

    //public void MakeStateAction()
    //{
    //    CurrentState.DoAction();
    //}


    public void MakeStateTransistion(IPlayerState playerStateTo)
    {
        CurrentState.OnStateExit();
        CurrentState = playerStateTo;
        CurrentState.OnStateEnter();
    }
    /// <summary>
    /// Same as above, but the new state's DoAction() method will also be called after OnStateExit/Enter
    /// </summary>
    /// <param name="playerStateTo"></param>
    //public void MakeStateTransistionWithAction(IPlayerState playerStateTo)
    //{
    //    MakeStateTransistion(playerStateTo);
    //    CurrentState.DoAction();
    //}

}
