using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Projectile of a dogpoo bag, doing damage and creating debuffing shitStain on impact
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-19
public class PooGrenadeProjectile : GroundEffectGrenade
{
	private PooGrenadeProjectile grenade;
	[SerializeField]
	private NegativeGroundObject groundObject;

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
		groundObject.CreateGroundObject(groundObjectPos, FOV.ViewRadius, healthModifyAmount, weaponEffects);
	}

	protected override void ImpactAgents()
	{
		List<GameObject> targetList = FOV.VisibleTargets;
		foreach (GameObject target in targetList)
		{
			if (target.GetComponent<Attributes>().Health > 0)
			{
				Vector3 explosionForceDirection = target.transform.position - transform.position;
				explosionForceDirection.y = 0;
				explosionForceDirection.Normalize();

				Effects.ApplyForce(target, explosionForceDirection * explosionForce);
				//Effects.ApplyWeaponEffects(target, weaponEffects);
				Effects.RegularDamage(target, healthModifyAmount, thrower);
			}
		}
		//AddToEffectList(groundObject);
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		Explode();
	}
}