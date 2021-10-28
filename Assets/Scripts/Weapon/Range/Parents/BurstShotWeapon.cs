using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all burst weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public class BurstShotWeapon : RangedWeapon
{
	private int bulletCount;

	public override void Attack(Animator animator)
	{
		base.Attack(animator);
		animator.SetTrigger("isRangedBurstShot");
		bulletCount = 4;
	}
	public override void DoAction(FieldOfView fov)
	{
		bulletCount--;
		
		if(bulletCount <= 0)
		{
			base.DoAction(fov);
		}
		else
		{
			Vector3 direction = transform.forward;
			direction.y = 0;
			direction.Normalize();

			bullet.GetComponent<Bullet>().CreateBullet(WeaponMuzzle.transform.position, direction, BulletFireForce, HitForce, Damage);
		}
	}
}
