using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class DeadState : AbstractPlayerState
{
    public override void TransitionState(IPlayerState state)
    {
        if (state.GetType() == typeof(ReviveState))//if ska inte va död längre
        {
			base.TransitionState(state);
        }
    }

    Color originalColor;
	public override void OnStateEnter()
	{
		Debug.Log("Enters DeadState" + this);
		//Die animation
		originalColor = GetComponent<MeshRenderer>().material.color;
		GetComponent<MeshRenderer>().material.color = Color.black;
	}

	public override void OnStateExit()
	{
		Debug.Log("Exits DeadState" + this);
		GetComponent<MeshRenderer>().material.color = originalColor;
	}
}
