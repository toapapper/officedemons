using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The harmful grenade version thrown by Devins special weapon
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-17
public class BadCoffeeGrenade : GroundEffectGrenade
{
	private BadCoffeeGrenade grenade;
	[SerializeField]
	private NegativeGroundObject coffeeStain;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce,
		float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<WeaponEffects> effects)
	{
		grenade = Instantiate(this, position, Quaternion.LookRotation(direction));
		grenade.thrower = thrower;
		grenade.FOV.ViewRadius = explodeRadius;
		grenade.healthModifyAmount = grenadeDamage;
		grenade.explosionForce = grenadeExplodeForce;
		grenade.weaponEffects = effects;
		grenade.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);

		GameManager.Instance.StillCheckList.Add(grenade.gameObject);
	}

	protected override void CreateGroundObject(Vector3 groundObjectPos)
	{
		coffeeStain.CreateGroundObject(groundObjectPos, FOV.ViewRadius, healthModifyAmount, weaponEffects);
	}

	protected override void ImpactAgents()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;
		foreach (GameObject target in targetList)
		{
			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			Effects.Damage(target, healthModifyAmount, thrower);
			Effects.ApplyForce(target, explosionForceDirection * explosionForce);
			Effects.ApplyWeaponEffects(target, weaponEffects);
		}
		AddToEffectList(coffeeStain);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.transform.tag == "Ground")
		{
			CreateGroundObject(collision.contacts[0].point);
		}
		else
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, LayerMask.GetMask("Ground")))
			{
				CreateGroundObject(hit.point);
			}
		}
		Explode();
	}
}
