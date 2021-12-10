using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all single shot weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public class SingleShotWeapon : RangedWeapon
{
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isRangedSingleShot");
		base.Attack(animator);
	}
	public override void DoAction(FieldOfView fov)
	{
		if (particleEffect)
		{
			Instantiate(particleEffect, WeaponMuzzle.transform.position, WeaponMuzzle.transform.rotation * Quaternion.Euler(0f, 0f, 0f));
			CameraShake.Shake(0.1f, 0.1f);
		}
		Vector3 direction = GetBulletDirection();
		bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, WeaponMuzzle.transform.position, direction, BulletFireForce, HitForce, Damage, this.effects);

		base.DoAction(fov);
	}
}
