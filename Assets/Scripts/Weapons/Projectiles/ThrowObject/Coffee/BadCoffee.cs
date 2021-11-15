using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCoffee : SpecialGrenade
{
	private BadCoffee specialGrenadeObject;
	protected List<WeaponEffects> effects;
	protected float grenadeDamage;
	protected float grenadeExplodeForce;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce, float explodeRadius, float grenadeExplodeForce, float grenadeDamage, List<WeaponEffects> effects)
	{
		specialGrenadeObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		specialGrenadeObject.thrower = thrower;
		specialGrenadeObject.GetComponent<FieldOfView>().ViewRadius = explodeRadius;
		specialGrenadeObject.grenadeDamage = grenadeDamage;
		specialGrenadeObject.grenadeExplodeForce = grenadeExplodeForce;
		specialGrenadeObject.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
		specialGrenadeObject.explodeTime = initialExplodeTime;
		GameManager.Instance.StillCheckList.Add(specialGrenadeObject.gameObject);

		specialGrenadeObject.effects = effects;
	}

	protected override void Explode()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;

		foreach (GameObject target in targetList)
		{
			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			Effects.RegularDamage(target, grenadeDamage/* * (1 + thrower.GetComponentInParent<StatusEffectHandler>().DmgBoost)*/, thrower);
			Effects.ApplyForce(target, explosionForceDirection * grenadeExplodeForce);
			Effects.ApplyWeaponEffects(target, effects);
		}

		Destroy(gameObject);
	}
}
