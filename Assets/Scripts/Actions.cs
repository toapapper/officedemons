using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// Contains actions that AI-agents can perform.
/// 
/// </para>
///   
///  <para>
///  Author: Tinea Larsson, Kristian Svennson, Jonas Lundin
///  
/// </para>
///  
/// </summary>

// Last Edited: 2021-10-14

public class Actions : MonoBehaviour
{
	private Attributes attributes;
	private CharacterController cc;
	private FieldOfView fov;

    void Start()
	{
		attributes = GetComponent<Attributes>();
		cc = GetComponent<CharacterController>();
		fov = GetComponent<FieldOfView>();
	}

	public void PickUp(AbstractWeapon weapon)
    {
		// TODO: Implement pickup
    }

	public void Attack(AbstractWeapon abstractWeapon)
	{
		List<GameObject> targetList = fov.visibleTargets;

		if (abstractWeapon is RangedWeapon)
		{
			if(targetList.Count > 0)
			{
				GameObject target = targetList[targetList.Count - 1];
				Attributes targetAttributes = target.GetComponent<Attributes>();
				targetAttributes.Health -= abstractWeapon.Damage;
			}
		}
		else if (abstractWeapon is MeleeWeapon)
		{
			foreach (GameObject target in targetList)
			{
				Attributes targetAttributes = target.GetComponent<Attributes>();
				targetAttributes.Health -= abstractWeapon.Damage;
			}
		}
	}

	public void Hit(Vector3 fromPosition, int damage, int force)
	{
		if (fov.visibleTargets.Count > 0)
		{
			List<GameObject> targetList = fov.visibleTargets;
			Vector3 hitForce = (fromPosition - transform.position).normalized * force;

			foreach (GameObject target in targetList)
			{
				Debug.Log(target);
				Attributes targetAttributes = target.GetComponent<Attributes>();
				targetAttributes.Health -= damage;
				target.GetComponent<Rigidbody>().AddForce(hitForce, ForceMode.VelocityChange);
			}
		}
	}

	public void TakeBulletDamage(int damage, Vector3 bulletHitForce)
	{
		attributes.Health -= damage;
		GetComponent<Rigidbody>().AddForce(bulletHitForce, ForceMode.VelocityChange);
	}

	public void Die()
	{
		if (this.tag == "Enemy")
		{
            GetComponent<MeshRenderer>().material.color = Color.black;
            gameObject.GetComponent<AIController>().CurrentState = AIStates.States.Dead;
        }
		else if (this.tag == "Player")
		{
			GetComponent<PlayerStateController>().Die();
			if (GameManager.Instance.combatState == CombatState.none)
				StartCoroutine("DelayedSelfRevive");
		}
	}
	IEnumerator DelayedSelfRevive()
    {
		yield return new WaitForSeconds(1);
		Revive(gameObject);
		yield return null;
    }

	public void Revive(GameObject target)
	{
		attributes.Health = attributes.StartHealth/2;
		target.GetComponent<PlayerStateController>().Revive();
	}

    public void MoveTowards(NavMeshAgent agent, GameObject target)
    {
		agent.isStopped = false;
		agent.SetDestination(target.transform.position);
        agent.gameObject.GetComponent<Attributes>().Stamina -= 1 * Time.deltaTime;
    }
}
