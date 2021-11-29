using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The harmful grenade version thrown by Devins special weapon,
/// doing damage and creating debuffing coffestain on impact
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-18
public class BadCoffeeGrenade : GroundEffectGrenade
{
	private BadCoffeeGrenade grenade;
	[SerializeField]
	private NegativeGroundObject coffeeStain;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 velocity,
		float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<WeaponEffects> effects)
	{
		grenade = Instantiate(this, position, Quaternion.LookRotation(velocity.normalized));
		grenade.thrower = thrower;
		grenade.FOV.ViewRadius = explodeRadius;
		grenade.healthModifyAmount = grenadeDamage;
		grenade.explosionForce = grenadeExplodeForce;
		grenade.weaponEffects = effects;
		grenade.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

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
			if(target.GetComponent<Attributes>().Health > 0)
			{
				if (target.layer != LayerMask.NameToLayer("Destructible"))
				{
					Vector3 explosionForceDirection = target.transform.position - transform.position;
					explosionForceDirection.y = 0;
					explosionForceDirection.Normalize();

					Effects.ApplyForce(target, explosionForceDirection * explosionForce);
					//Effects.ApplyWeaponEffects(target, weaponEffects);
					Effects.WeaponDamage(target, healthModifyAmount, thrower);
				}
				else
				{
					Effects.Damage(target, healthModifyAmount);
				}
			}
		}
		//AddToEffectList(coffeeStain);
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		AkSoundEngine.PostEvent("Play_Splash", gameObject);
		Explode();
	}
}
