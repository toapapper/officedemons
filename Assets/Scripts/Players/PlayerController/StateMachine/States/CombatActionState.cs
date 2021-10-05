using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CombatActionState : AbstractPlayerState
{
	public override void LockAction() { }
	public override void CancelAction() { }
	public override void OnAttack() { }
	public override void OnSpecial() { }
	public override void OnPickUp(GameObject weapon) { }
	public override void OnStartThrow() { }
	public override void OnThrow() { }
	public override void OnRevive(GameObject player) { }

	public override void OnFixedUpdateState() { }


	public override void OnStateEnter()
	{
		Debug.Log("Enters CombatActionState" + this + " Action: " + GetComponent<CombatTurnState>().ChosenAction);
		switch (GetComponent<CombatTurnState>().ChosenAction)
		{
			case TypeOfAction.ATTACK:
				weaponHand.Attack();
				break;
			case TypeOfAction.SPECIALATTACK:
				//specialHand.Attack();
				break;
			case TypeOfAction.THROW:
				playerMovement.PerformThrow();
				break;
			case TypeOfAction.REVIVE:
				GetComponent<CombatTurnState>().PlayerToRevive.GetComponentInChildren<Attributes>().Health = 100;
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
				PlayerManager.doneEvent.Invoke();
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
