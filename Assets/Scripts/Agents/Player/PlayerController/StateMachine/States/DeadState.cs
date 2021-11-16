using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Character can not do anything during this state. Additionally it restricts state transitions when in combat.
/// </para> 
/// 
///  <para>
///  Author: Ossian
/// </para>
/// </summary>

// Last Edited: 2021-10-22

public class DeadState : AbstractPlayerState
{
	/// <summary>
	/// Simple override of the base that only calls the base if the criteria is met
	/// </summary>
	/// <param name="state"></param>
    public override void TransitionState(IPlayerState state)
    {
        if (state is ReviveState || state is OutOfCombatState)
        {
			base.TransitionState(state);
        }
    }

	/// <summary>
	/// Simple coroutine reviving this.gameObject after one second
	/// </summary>
	/// <returns></returns>
	IEnumerator DelayedSelfRevive()
	{
		yield return new WaitForSeconds(1);
		Effects.Revive(gameObject);
		yield return null;
	}

	private Color originalColor; //is here temporarily i assume. This is because we have no proper animation to show one is dead other than to change the color
	public override void OnStateEnter()
	{
		Debug.Log("Enters DeadState" + this);
		originalColor = GetComponentInChildren<MeshRenderer>().material.color;

		Effects.Disarm(gameObject);

		GetComponentInChildren<MeshRenderer>().material.color = Color.black;
		if (GameManager.Instance.CurrentCombatState == CombatState.none)
			StartCoroutine(DelayedSelfRevive());
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits DeadState" + this);
		GetComponentInChildren<MeshRenderer>().material.color = originalColor;
	}
}
