using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	int bulletDamage;
	Vector3 bulletHitForce;
	private Bullet bulletObject;

	public void CreateBullet(Vector3 position, Vector3 direction, int bulletFireForce, int bulletHitForce, int bulletDamage)
	{
		bulletObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		bulletObject.bulletDamage = bulletDamage;
		bulletObject.bulletHitForce = direction * bulletHitForce;
		bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.VelocityChange);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger)
		{
			if(other.tag == "Player" || other.tag == "Enemy")
			{
				Effects.Damage(other.gameObject, bulletDamage);
				Effects.ApplyForce(other.gameObject, bulletHitForce);
			}

			Destroy(gameObject);
		}
	}
}
