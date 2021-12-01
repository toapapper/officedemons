using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// TODO: Make the character move to correct position before enetering combat
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-12
public class CombatEnterState : AbstractPlayerState
{
	public override void OnFixedUpdateState()
	{
		if (playerMovement.AtDestination())
		{
			PlayerManager.Instance.PlayerAtCombatPosition(this.gameObject);
		}
	}


	public override void OnStateEnter()
	{
		inputHandler.LockInput();

		Debug.Log("Enters CombatEnterState" + this);
		int i = PlayerManager.players.FindIndex(gameObject => gameObject == this.gameObject);
		Vector3 movePosition = GameManager.Instance.CurrentEncounter.playerPositions[i].transform.position;
		movePosition.y = transform.position.y;
		if(GetComponent<Attributes>().Health > 0)
		{
			playerMovement.MoveTo(movePosition);
		}
	}


	public override void OnStateExit()
	{
		Debug.Log("Exits CombatEnterState" + this);
		playerMovement.ResetNavMeshPath();

		inputHandler.ResetInput();
	}
}
