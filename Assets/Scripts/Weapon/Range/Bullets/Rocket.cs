using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
	private List<GameObject> targetList = new List<GameObject>();

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Enemy")
		{
			targetList.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Enemy")
		{
			targetList.Remove(other.gameObject);
		}
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		if(targetList.Count > 0)
		{
			foreach (GameObject target in targetList)
			{
				if (target.tag == "Player" || target.tag == "Enemy")
				{
					Effects.Damage(collision.gameObject, bulletDamage);
					Effects.ApplyForce(collision.gameObject, (target.transform.position - transform.position).normalized * bulletHitForce);
				}
			}
		}

		//TODO Explosion
		Destroy(gameObject);
	}
}
