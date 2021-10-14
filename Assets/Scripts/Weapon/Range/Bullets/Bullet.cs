using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	protected int bulletDamage;
	protected Vector3 bulletHitForce;
	private Bullet bulletObject;

	public void CreateBullet(Vector3 position, Vector3 direction, int bulletFireForce, int bulletHitForce, int bulletDamage)
	{
		bulletObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		bulletObject.bulletDamage = bulletDamage;
		Debug.Log(bulletHitForce);
		bulletObject.bulletHitForce = direction * bulletHitForce;
		bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.VelocityChange);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger)
		{
			if (other.gameObject.GetComponentInParent<Actions>() != null)
			{
				other.gameObject.GetComponentInParent<Actions>().TakeBulletDamage(bulletDamage, bulletHitForce);
			}
			Destroy(gameObject);
		}
	}
}
