using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEffectBombardWeapon : BombardWeapon
{
	public override void DoAction(FieldOfView fov)
	{
		Vector3 forward = transform.forward;
		forward.y = 0;
		forward.Normalize();
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 direction = (Quaternion.AngleAxis(-GetComponentInParent<WeaponHand>().ThrowAim.initialAngle, right) * forward).normalized;
		float throwForce = GetComponentInParent<WeaponHand>().ThrowAim.initialVelocity;
		grenade.GetComponent<PooGrenadeProjectile>().CreateGrenade(holderAgent, transform.position, direction, throwForce, explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);

		base.DoAction(fov);
	}
}
