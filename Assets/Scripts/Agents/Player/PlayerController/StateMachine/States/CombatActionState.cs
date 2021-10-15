using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Makes the character perfom the actions chosen in the combat turn state
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-12
public class CombatActionState : AbstractPlayerState
{
	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatActionState" + this + " Action: " + GetComponent<CombatTurnState>().ChosenAction);
		switch (GetComponent<CombatTurnState>().ChosenAction)
		{
			case TypeOfAction.ATTACK:
				weaponHand.Attack();
				break;
			case TypeOfAction.BOMBARD:
				weaponHand.PerformBombard();
				break;
			case TypeOfAction.SPECIALATTACK:
				//specialHand.Attack();
				break;
			case TypeOfAction.THROW:
				weaponHand.Throw();
				break;
			case TypeOfAction.REVIVE:
				GetComponent<Actions>().Revive(GetComponent<CombatTurnState>().PlayerToRevive);
				Debug.LogWarning("combatActionState revive " + GetComponent<CombatTurnState>().PlayerToRevive);
				break;
			case TypeOfAction.NOACTION:
				break;
		}
		Debug.LogWarning("Reset action");
		GetComponent<CombatTurnState>().ChosenAction = TypeOfAction.NOACTION;

		StartCoroutine("WaitDone");
	}

	IEnumerator WaitDone()
    {
		yield return new WaitForSeconds(1);
        while (true)
        {
			Debug.Log("CombatActionState Coroutine");
			if (GameManager.Instance.AllStill)
			{
				PlayerManager.Instance.NextPlayerAction();
				GetComponent<PlayerStateController>().StartWaitForTurn();
				StopCoroutine("WaitDone");
			}
			yield return null;
        }
    }

	public override void OnStateExit()
	{
		Debug.Log("Exits CombatActionState" + this);
	}
}