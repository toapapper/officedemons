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

	//// Update is called once per frame
	//void Update()
	//{

	//}

	//public void PickUp(AbstractWeapon weapon)
 //   {
	//	//gör något
 //   }

	//public void Attack(AbstractWeapon abstractWeapon)
	//{
	//	//Currently Equipped
	//	//weapon = GetComponent<Weapon>();
	//	//weapon.damage;

	//	List<GameObject> targetList = fov.visibleTargets;

	//	if (abstractWeapon is RangedWeapon)
	//	{
	//		if(targetList.Count > 0)
	//		{
	//			GameObject target = targetList[targetList.Count - 1];
	//			Attributes targetAttributes = target.GetComponent<Attributes>();
	//			targetAttributes.Health -= abstractWeapon.Damage;
	//		}
	//	}
	//	else if (abstractWeapon is MeleeWeapon)
	//	{
	//		foreach (GameObject target in targetList)
	//		{
	//			Attributes targetAttributes = target.GetComponent<Attributes>();
	//			targetAttributes.Health -= abstractWeapon.Damage;
	//		}
	//	}
	//}

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
			if (GameManager.Instance.combatState == CombatState.none)
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
