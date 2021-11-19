using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The explosive grenade, detonating when it stops
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-19
public class ExplosiveGrenadeProjectile : GrenadeProjectile
{
    private ExplosiveGrenadeProjectile grenade;

	[SerializeField]
	private GameObject FOVVisualization;
	[SerializeField]
	private float explodeTime = 1f;

	public virtual void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce,
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

	protected override void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				SetExplosion();
			}
			base.FixedUpdate();
		}
		else
		{
			isObjectThrown = true;
		}
	}

	private void SetExplosion()
	{
		FOVVisualization.SetActive(true);
		StartCoroutine(CountdownTime(explodeTime));
	}

	protected override void ImpactAgents()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;

		foreach (GameObject target in targetList)
		{
			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			Effects.RegularDamage(target, healthModifyAmount, thrower);
			Effects.ApplyForce(target, explosionForceDirection * explosionForce);
			Effects.ApplyWeaponEffects(target, weaponEffects);
		}
	}
}
