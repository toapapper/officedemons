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

	public virtual void CreateGrenade(GameObject thrower, Vector3 position, Vector3 velocity,
		float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<StatusEffectType> effects)
	{
		grenade = Instantiate(this, position, Quaternion.LookRotation(velocity.normalized));
		grenade.thrower = thrower;
		grenade.FOV.ViewRadius = explodeRadius;
		grenade.healthModifyAmount = grenadeDamage;
		grenade.explosionForce = grenadeExplodeForce;
		grenade.weaponEffects = effects;
		grenade.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
        grenade.tag = "Projectile"; // Tinea 30/11

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

	protected override void Explode()
    {
		AkSoundEngine.PostEvent("Play_Explosion", gameObject);
		CameraShake.Shake(0.5f, 0.5f);
		base.Explode();
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
			if (target.GetComponent<Attributes>().Health > 0)
			{
				if (target.layer != LayerMask.NameToLayer("Destructible"))
				{
					Vector3 explosionForceDirection = target.transform.position - transform.position;
					explosionForceDirection.y = 0;
					explosionForceDirection.Normalize();

					Effects.RegularWeaponDamage(target, healthModifyAmount, thrower);
					Effects.ApplyForce(target, explosionForceDirection * explosionForce);
					Effects.ApplyWeaponEffects(target, thrower, weaponEffects);
				}
				else
				{
					Effects.Damage(target, healthModifyAmount, thrower);
				}
			}
		}
	}
}
