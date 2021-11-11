using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactGrenade : GrenadeObject
{
	private void OnCollisionEnter(Collision collision)
	{
		//TODO if ground
		//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
		//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
		//base.SetExplosion();
		Explode();
	}

	private void Explode()
	{
		//thrower.GetComponent<StatusEffectHandler>().DmgBoost;
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;

		foreach (GameObject target in targetList)
		{
			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			Effects.Damage(target, grenadeDamage * (1 + thrower.GetComponentInParent<StatusEffectHandler>().DmgBoost), thrower);
			Effects.ApplyForce(target, explosionForceDirection * grenadeExplodeForce);
			Effects.ApplyWeaponEffects(target, effects);
		}

		Destroy(gameObject);
	}
}
