using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The bullet object/projectile
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public class Bullet : MonoBehaviour
{
	protected float bulletDamage;
	protected Vector3 bulletHitForce;
	private Bullet bulletObject;

	public List<WeaponEffects> effects;

	public void CreateBullet(Vector3 position, Vector3 direction, float bulletFireForce, float bulletHitForce, float bulletDamage, List<WeaponEffects> effects)
	{
		bulletObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		bulletObject.bulletDamage = bulletDamage;
		bulletObject.bulletHitForce = direction * bulletHitForce;
		bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.VelocityChange);
		GameManager.Instance.StillCheckList.Add(bulletObject.gameObject);

		bulletObject.effects = effects;
	}

	private void FixedUpdate()
	{
		Vector2 viewpos = Camera.main.WorldToViewportPoint(transform.position);
		if (viewpos.x > 1 || viewpos.x < 0 || viewpos.y > 1 || viewpos.y < 0)
		{
			Destroy(gameObject);
		}
	}

    protected virtual void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
		{
			Effects.Damage(collision.gameObject, bulletDamage);
			Effects.ApplyForce(collision.gameObject, bulletHitForce);

			Effects.ApplyWeaponEffects(collision.gameObject, effects);
		}

		Destroy(gameObject);
	}
}
