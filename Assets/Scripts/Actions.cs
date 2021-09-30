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
		if (Mathf.Abs(cc.velocity.x) > 0 || Mathf.Abs(cc.velocity.y) > 0)
		{
			attributes.Stamina -= 1 * Time.deltaTime;
		}

		if (attributes.Health <= 0)
		{
			Die();
		}
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

	public void Hit(int damage)
	{
		List<GameObject> targetList = fov.visibleTargets;

		foreach (GameObject target in targetList)
		{
			Attributes targetAttributes = target.GetComponent<Attributes>();
			targetAttributes.Health -= damage;
		}
	}

	void Die()
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
		}
	}

	void Revive(GameObject target)
	{
		Attributes targetAttributes = target.GetComponent<Attributes>();
		targetAttributes.Health = targetAttributes.StartHealth;
	}

    void MoveTowards(NavMeshAgent agent, NavMeshAgent target)
    {
        //if agent.stamina != 0
        float agentStamina = agent.gameObject.GetComponent<Attributes>().Stamina;

        if (agentStamina > 0)
        {
            agent.SetDestination(target.transform.position);
        }
    }
}
