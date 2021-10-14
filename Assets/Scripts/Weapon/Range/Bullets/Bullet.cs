using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	float bulletDamage;
	Vector3 bulletHitForce;
	private Bullet bulletObject;

	public void CreateBullet(Vector3 position, Vector3 direction, float bulletFireForce, float bulletHitForce, float bulletDamage)
	{
		bulletObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		bulletObject.bulletDamage = bulletDamage;
		bulletObject.bulletHitForce = direction * bulletHitForce;
		bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.VelocityChange);
		GameManager.Instance.stillCheckList.Add(bulletObject.gameObject);
	}

	private void FixedUpdate()
	{
		Vector2 viewpos = Camera.main.WorldToViewportPoint(transform.position);
		if (viewpos.x > 1 || viewpos.x < 0 || viewpos.y > 1 || viewpos.y < 0)
		{
			Destroy(gameObject);
		}
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
