using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The rocket object/projectile
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-28
public class Rocket : Bullet
{
	private List<GameObject> targetList = new List<GameObject>();
	[SerializeField]
	private GameObject explosion;
    private void Start()
    {
		gameObject.GetComponentInChildren<ParticleSystem>().Stop();
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
		Instantiate(explosion, transform.position, transform.rotation);
		//gameObject.GetComponentInChildren<ParticleSystem>().Play();
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Enemy")
		{
			if (!targetList.Contains(other.gameObject))
			{
				targetList.Add(other.gameObject);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Enemy")
		{
			targetList.Remove(other.gameObject);
		}
	}
}
