using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class PlayerStateController : MonoBehaviour
{
    private PlayerStateContext playerContext;
    private Dictionary<PlayerStates, IPlayerState> states;

    private int playerNr;

    public void StartOutOfCombat() => playerContext.MakeStateTransistion(states[PlayerStates.OUTOFCOMBAT]);
    public void StartCombat() => playerContext.MakeStateTransistion(states[PlayerStates.ENTERCOMBAT]);
    public void StartTurn() => playerContext.MakeStateTransistion(states[PlayerStates.COMBATTURN]);
    public void StartCombatAction() => playerContext.MakeStateTransistion(states[PlayerStates.COMBATACTION]);
    public void StartWaitForTurn() => playerContext.MakeStateTransistion(states[PlayerStates.COMBATWAIT]);
    public void Die() => playerContext.MakeStateTransistion(states[PlayerStates.DEAD]);

    private void Awake()
    {
        SetupStates();
        playerContext = new PlayerStateContext(states[PlayerStates.OUTOFCOMBAT]);
    }

	void OnEnable()
	{
		if (PlayerManager.players == null)
			PlayerManager.players = new List<GameObject>();

        Debug.Log("Added me!");

		PlayerManager.players.Add(this.gameObject);
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


    public void OnMove(CallbackContext context)
    {
        playerContext.OnMove(context);
    }
    public void OnAttack(CallbackContext context)
    {
        playerContext.OnAttack(context);
    }
    public void OnSpecial(CallbackContext context)
    {
        playerContext.OnSpecial(context);
    }
    public void OnPickupThrow(CallbackContext context)
    {
        playerContext.OnPickupThrow(context);
    }
    public void OnRevive(CallbackContext context)
    {
        playerContext.OnRevive(context);
    }

    private void FixedUpdate() => playerContext.FixedUpdateContext();
}
