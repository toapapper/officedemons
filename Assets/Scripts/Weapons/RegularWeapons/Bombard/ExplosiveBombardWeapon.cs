using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBombardWeapon : BombardWeapon
{
	public override void DoAction(FieldOfView fov)
	{
		Vector3 forward = transform.forward;
		forward.y = 0;
		forward.Normalize();
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 direction = (Quaternion.AngleAxis(-WeaponController.ThrowAim.initialAngle, right) * forward).normalized;
		float throwForce = WeaponController.ThrowAim.ThrowForce;
		//float throwForce = GetComponentInParent<WeaponHand>().ThrowAim.initialVelocity;
		//float throwForce = GetComponentInParent<WeaponHand>().ThrowAim.ThrowForce;

		grenade.GetComponent<ExplosiveGrenadeProjectile>().CreateGrenade(HolderAgent, transform.position, direction, throwForce,
			explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);

		base.DoAction(fov);
	}
}
