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

    private Dictionary<PlayerStates, IPlayerState> states;


    public void StartOutOfCombat() => MakeStateTransistion(states[PlayerStates.OUTOFCOMBAT]);
    public void StartCombat() => MakeStateTransistion(states[PlayerStates.ENTERCOMBAT]);
    public void StartTurn() => MakeStateTransistion(states[PlayerStates.COMBATTURN]);
    public void StartCombatAction() => MakeStateTransistion(states[PlayerStates.COMBATACTION]);
    public void StartWaitForTurn() => MakeStateTransistion(states[PlayerStates.COMBATWAIT]);
    public void Die() => MakeStateTransistion(states[PlayerStates.DEAD]);

    private void Awake()
    {
        SetupStates();
        playerContext = new PlayerStateContext(states[PlayerStates.OUTOFCOMBAT]);
		PlayerManager.players.Add(this.gameObject);
    }

	void OnEnable()
	{
		if (PlayerManager.players == null)
			PlayerManager.players = new List<GameObject>();

		playerNr = PlayerManager.players.Count;
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
	}

    public void MakeStateTransistion(IPlayerState playerStateTo)
    {
        CurrentState.OnStateExit();
        CurrentState = playerStateTo;
        CurrentState.OnStateEnter();
    }
    //public void OnMove(CallbackContext context)
    //{
    //    CurrentState.OnMove(context);
    //}
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
    public void OnSpecial()
    {
        CurrentState.OnSpecial();
    }
    public void OnPickUp(GameObject weapon)
	{
        CurrentState.OnPickUp(weapon);
    }
    public void OnThrow(CallbackContext context)
	{
        CurrentState.OnThrow(context);
	}
    public void OnRevive(CallbackContext context)
    {
        CurrentState.OnRevive(context);
    }

    private void FixedUpdate() => CurrentState.OnFixedUpdateState();
}
