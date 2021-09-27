using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerStateContext playerContext;
    private Dictionary<PlayerStates, IPlayerState> states;

    public void StartOutOfCombat() => playerContext.MakeStateTransistion(states[PlayerStates.OUTOFCOMBAT]);
    public void StartCombat() => playerContext.MakeStateTransistion(states[PlayerStates.ENTERCOMBAT]);
    public void StartTurn() => playerContext.MakeStateTransistion(states[PlayerStates.COMBATTURN]);
    public void StartWaitForTurn() => playerContext.MakeStateTransistion(states[PlayerStates.COMBATWAIT]);
    public void Die() => playerContext.MakeStateTransistion(states[PlayerStates.DEAD]);

    private void Awake()
    {
        playerContext = new PlayerStateContext(this);
        SetupStates();
    }

    private void Start()
    {
        //set player in idle state to begin
        playerContext.MakeStateTransistion(states[PlayerStates.OUTOFCOMBAT]);
    }

    private void SetupStates()
    {
        //Add all new states here. 
        states = new Dictionary<PlayerStates, IPlayerState>();
        states.Add(PlayerStates.OUTOFCOMBAT, gameObject.AddComponent<OutOfCombatState>());
        states.Add(PlayerStates.ENTERCOMBAT, gameObject.AddComponent<CombatEnterState>());
        states.Add(PlayerStates.COMBATTURN, gameObject.AddComponent<CombatTurnState>());
        states.Add(PlayerStates.COMBATWAIT, gameObject.AddComponent<CombatWaitState>());
        states.Add(PlayerStates.DEAD, gameObject.AddComponent<DeadState>());
    }
}
