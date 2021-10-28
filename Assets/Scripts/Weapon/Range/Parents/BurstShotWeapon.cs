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

// Last Edited: 14/10-28
public class BurstShotWeapon : RangedWeapon
{
	private int bulletCount;

	public override void Attack(Animator animator)
	{
		bulletCount = 4;
		animator.SetTrigger("isRangedBurstShot");
		base.Attack(animator);
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
			GameObject wielder = gameObject.GetComponentInParent<Attributes>().gameObject;
			if (wielder == null)
			{
				return;
			}

			Vector3 direction = transform.forward;
			direction.y = 0;
			direction.Normalize();

			Debug.Log("doaction ranged weapon " + effects);
			bullet.GetComponent<Bullet>().CreateBullet(WeaponMuzzle.transform.position, direction, BulletFireForce, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), this.effects);

			//recoil and slippery-checks

			//Check for recoil recoil deals half the weapondamage and applies the effects
			if (effects.Contains(WeaponEffects.Recoil))
			{
				float rand = Random.value;
				if (rand < RecoilChance)
				{
					Effects.Damage(wielder, Damage / 2);
					Effects.ApplyForce(wielder, (wielder.transform.forward * -1 * HitForce));
					Effects.ApplyWeaponEffects(wielder, effects);
				}
			}

			//disarms the wielder
			if (effects.Contains(WeaponEffects.Slippery))
			{
				float rand = Random.value;
				if (rand < SlipperyDropChance)
				{
					Effects.Disarm(wielder);
				}
			}
		}
	}
}
