using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class PlayerStateController : MonoBehaviour
{
    public IPlayerState CurrentState
    {
        get;
        set;
    }

    public Dictionary<PlayerStates, IPlayerState> states { get; private set; }


    public void StartOutOfCombat() => MakeStateTransition(states[PlayerStates.OUTOFCOMBAT]);
    public void StartCombat() => MakeStateTransition(states[PlayerStates.ENTERCOMBAT]);
    public void StartTurn() => MakeStateTransition(states[PlayerStates.COMBATTURN]);
    public void StartCombatAction() => MakeStateTransition(states[PlayerStates.COMBATACTION]);
    public void StartWaitForTurn() => MakeStateTransition(states[PlayerStates.COMBATWAIT]);
    public void Die() => MakeStateTransition(states[PlayerStates.DEAD]);
    public void Revive() => MakeStateTransition(states[PlayerStates.REVIVE]);

    private void Start()
    {
        SetupStates();
        CurrentState = states[PlayerStates.OUTOFCOMBAT];

        if (PlayerManager.players == null)
            PlayerManager.players = new List<GameObject>();
        PlayerManager.players.Add(this.gameObject);

        if(GameManager.Instance.stillCheckList == null)
            GameManager.Instance.stillCheckList = new List<GameObject>();
        GameManager.Instance.stillCheckList.Add(gameObject);
    }

    private void SetupStates()
    {
        //Add all new states here.
        states = new Dictionary<PlayerStates, IPlayerState>();
        states.Add(PlayerStates.OUTOFCOMBAT, gameObject.AddComponent<OutOfCombatState>());
		states.Add(PlayerStates.ENTERCOMBAT, gameObject.AddComponent<CombatEnterState>());
		states.Add(PlayerStates.COMBATTURN, gameObject.AddComponent<CombatTurnState>());
        states.Add(PlayerStates.COMBATACTION, gameObject.AddComponent<CombatActionState>());
		states.Add(PlayerStates.COMBATWAIT, gameObject.AddComponent<CombatWaitState>());
		states.Add(PlayerStates.DEAD, gameObject.AddComponent<DeadState>());
        states.Add(PlayerStates.REVIVE, gameObject.AddComponent<ReviveState>());
	}

    public void MakeStateTransition(IPlayerState playerStateTo)
    {
        CurrentState.TransitionState(playerStateTo);
    }

    public void LockAction()
	{
        CurrentState.LockAction();
	}
    public void CancelAction()
	{
        CurrentState.CancelAction();
	}
    public void OnAttack()
    {
        CurrentState.OnAttack();
    }
    public void OnStartBombard()
	{
        CurrentState.OnStartBombard();
    }
    public void OnBombard()
	{
        CurrentState.OnBombard();

    }
    public void OnSpecial()
    {
        CurrentState.OnSpecial();
    }
    public void OnPickUp(GameObject weapon)
	{
        CurrentState.OnPickUp(weapon);
    }
    public void OnStartThrow()
	{
        CurrentState.OnStartThrow();
	}
    public void OnThrow()
	{
        CurrentState.OnThrow();
	}
    public void OnRevive(GameObject player)
    {
        CurrentState.OnRevive(player);
    }

    private void FixedUpdate() => CurrentState.OnFixedUpdateState();
}
