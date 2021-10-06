using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// finds the proper state to transition into when being revived.
/// </summary>
class ReviveState : AbstractPlayerState
{
    public override void OnStateEnter()
    {
        Debug.Log("Entered ReviveState: " + gameObject.ToString());

        PlayerStates toState = PlayerStates.DEAD;
        if(GameManager.Instance.combatState == CombatState.none)
        {
            attributes.Health = attributes.StartHealth;
            toState = PlayerStates.OUTOFCOMBAT;
        }
        else// if(GameManager.Instance.combatState == CombatState.playerActions || GameManager.combatState.Enemy)
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