using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : AbstractPlayerState
{
    public override void TransitionState(IPlayerState state)
    {
        if (state.GetType() == typeof(ReviveState))//if ska inte va död längre
        {
			base.TransitionState(state);
        }
    }

	IEnumerator DelayedSelfRevive()
	{
		Debug.Log("DelayedSelfrevive");
		yield return new WaitForSeconds(1);
		Debug.Log("DelayedSelfrevive");
		Effects.Revive(gameObject);
		yield return null;
	}

	Color originalColor;
	public override void OnStateEnter()
	{
		Debug.Log("Enters DeadState" + this);
		//Die animation
		originalColor = GetComponent<MeshRenderer>().material.color;
		GetComponent<MeshRenderer>().material.color = Color.black;

		if (GameManager.Instance.CurrentCombatState == CombatState.none)
			StartCoroutine(DelayedSelfRevive());
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits DeadState" + this);
		GetComponent<MeshRenderer>().material.color = originalColor;
	}
}
