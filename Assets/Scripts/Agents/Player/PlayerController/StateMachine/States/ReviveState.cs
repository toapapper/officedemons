using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// <para>
/// Simply a transitory state that finds the proper state to revive into
/// </para> 
/// 
/// <para>
/// Author: Ossian
/// </para>
/// </summary>

// Last Edited: 2021-10-12
class ReviveState : AbstractPlayerState
{
    /// <summary>
    /// Goes either to combatwaitstate with 50% health or to outofcombatstate depending on GameManager.Instance.CurrentCombatState
    /// </summary>
    public override void OnStateEnter()
    {
        Debug.Log("Entered ReviveState: " + gameObject.ToString());

        PlayerStates toState = PlayerStates.DEAD;
        if(GameManager.Instance.CurrentCombatState == CombatState.none)
        {
            attributes.Health = attributes.StartHealth;
            toState = PlayerStates.OUTOFCOMBAT;
        }
        else
        {
            attributes.Health = attributes.StartHealth / 2;
            toState = PlayerStates.COMBATWAIT;
        }

        PlayerStateController controller = GetComponent<PlayerStateController>();
        IPlayerState st;
        controller.states.TryGetValue(toState, out st);
        controller.MakeStateTransition(st);
    }

    public override void OnStateExit()
    {
        Debug.Log("Exit ReviveState: " + gameObject.ToString());
    }
}