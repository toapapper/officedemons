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

	// Update is called once per frame
	void Update()
	{

	}

	public void PickUp(AbstractWeapon weapon)
    {
		//gör något
    }

	public void Attack(AbstractWeapon abstractWeapon)
	{
		//Currently Equipped
		//weapon = GetComponent<Weapon>();
		//weapon.damage; 

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
			//Destroy GameObject
		}
		else if (this.tag == "Player")
		{
			//Disable Movement
			//Play death animation
			// bool targetIsDead so it's not targetet and attacked again while dead 

			GetComponent<PlayerStateController>().Die();
			if (GameManager.Instance.combatState == CombatState.none)
				StartCoroutine("DelayedSelfRevive");
		}
	}
	IEnumerator DelayedSelfRevive()
    {
		Debug.Log("DelayedSelfrevive");
		yield return new WaitForSeconds(1);
		Debug.Log("DelayedSelfrevive");
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
