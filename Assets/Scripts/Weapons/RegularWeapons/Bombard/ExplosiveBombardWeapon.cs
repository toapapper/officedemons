using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBombardWeapon : BombardWeapon
{
	public override void DoAction(FieldOfView fov)
	{
		Vector3 velocity = WeaponController.ThrowAim.InitialVelocity;

		grenade.GetComponent<ExplosiveGrenadeProjectile>().
			CreateGrenade(HolderAgent, transform.position, velocity,
						  explodeRadius, HitForce, 
						  Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost),
						  Utilities.ListDictionaryKeys(effects));


		base.DoAction(fov);
	}
}
