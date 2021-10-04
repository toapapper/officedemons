using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	float bulletForce = 1f;
	int bulletDamage;
	private Bullet bulletObject;

	public void CreateBullet(Vector3 position, Vector3 direction, int bulletDamage)
	{
		this.bulletDamage = bulletDamage;
		bulletObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletForce, ForceMode.VelocityChange);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Collision triggered");
		if (other.gameObject.GetComponent<Actions>() != null)
		{
			other.gameObject.GetComponent<Actions>().TakeBulletDamage(bulletDamage, transform.position);
		}
		Destroy(this.gameObject);
	}
}
