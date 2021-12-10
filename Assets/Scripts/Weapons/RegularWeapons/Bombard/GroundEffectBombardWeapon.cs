using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEffectBombardWeapon : BombardWeapon
{
	public override void DoAction(/*FieldOfView fov*/)
	{
		Vector3 velocity = WeaponController.ThrowAim.InitialVelocity;

		grenade.GetComponent<PooGrenadeProjectile>().CreateGrenade(HolderAgent, transform.position, velocity,
			explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);

		base.DoAction(/*fov*/);
	}
}
