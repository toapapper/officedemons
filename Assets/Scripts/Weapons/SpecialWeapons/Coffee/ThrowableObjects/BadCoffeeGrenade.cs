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

// Last Edited: 15-11-16
public class BadCoffeeGrenade : CoffeeGrenade
{
	private BadCoffeeGrenade coffeeGrenade;
	[SerializeField]
	private BadCoffeeStain coffeeStain;
	protected List<WeaponEffects> effects;
	protected float grenadeDamage;
	protected float grenadeExplodeForce;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce, float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<WeaponEffects> effects)
	{
		coffeeGrenade = Instantiate(this, position, Quaternion.LookRotation(direction));
		coffeeGrenade.thrower = thrower;
		coffeeGrenade.GetComponent<FieldOfView>().ViewRadius = explodeRadius;
		coffeeGrenade.grenadeDamage = grenadeDamage;
		coffeeGrenade.grenadeExplodeForce = grenadeExplodeForce;
		coffeeGrenade.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
		coffeeGrenade.effects = effects;

		GameManager.Instance.StillCheckList.Add(coffeeGrenade.gameObject);
	}

	protected override void Explode()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;
		coffeeStain.CreateStain(transform.position, GetComponent<FieldOfView>().ViewRadius, grenadeDamage, effects);

		foreach (GameObject target in targetList)
		{
			coffeeStain.agentsOnStain.Add(target);

			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			Effects.Damage(target, grenadeDamage, thrower);
			Effects.ApplyForce(target, explosionForceDirection * grenadeExplodeForce);
			//Effects.ApplyWeaponEffects(target, effects);
		}

		Destroy(gameObject);
	}
}
