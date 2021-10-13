using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actions : MonoBehaviour
{
	//[SerializeField]
	Attributes attributes;
	CharacterController cc;
	FieldOfView fov;

    // Start is called before the first frame update
    void Start()
	{
		attributes = GetComponent<Attributes>();
		cc = GetComponent<CharacterController>();
		fov = GetComponent<FieldOfView>();
	}

	public void Die()
	{
		GameObject target = this.gameObject;
		if (this.tag == "Enemy")
		{
            // Tillfällig
            Debug.Log("Enemy died");
            GetComponent<MeshRenderer>().material.color = Color.black;
            gameObject.GetComponent<AIController>().CurrentState = AIStates.States.Dead;
        }
		else if (this.tag == "Player")
		{
			//Disable Movement
			//Play death animation
			// bool targetIsDead so it's not targetet and attacked again while dead

			GetComponent<PlayerStateController>().Die();
			if (GameManager.Instance.CurrentCombatState == CombatState.none)
				StartCoroutine(DelayedSelfRevive(target));
		}
	}
	IEnumerator DelayedSelfRevive(GameObject target)
    {
		Debug.Log("DelayedSelfrevive");
		yield return new WaitForSeconds(1);
		Debug.Log("DelayedSelfrevive");
		Revive(target);
		yield return null;
    }

	public void Revive(GameObject target)
	{
		target.GetComponent<PlayerStateController>().Revive();
	}

    public void MoveTowards(NavMeshAgent agent, GameObject target)
    {
		agent.isStopped = false;

		agent.SetDestination(target.transform.position);
        agent.gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
    }
}
