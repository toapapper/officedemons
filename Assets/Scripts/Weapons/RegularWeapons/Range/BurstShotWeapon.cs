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
    [SerializeField]
    private int bulletsInBurst = 4;
    public int BulletsInBurst 
    { 
        get { return bulletsInBurst; } 
        set { bulletsInBurst = value; }
    
    }

    public override void Attack(Animator animator)
    {
        bulletCount = bulletsInBurst;
        animator.SetTrigger("isRangedBurst");
        base.Attack(animator);
    }

    public override void DoAction(FieldOfView fov)
    {
        if(bulletCount > 0)
		{
            if (particleEffect)
            {
                Instantiate(particleEffect, WeaponMuzzle.transform.position, WeaponMuzzle.transform.rotation * Quaternion.Euler(0, 180, 0));
            }
            Vector3 direction = GetBulletDirection();
            bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, WeaponMuzzle.transform.position, direction, BulletFireForce, HitForce, Damage, this.effects);
            if (bulletCount == bulletsInBurst)
            {
                WeaponController.Animator.SetTrigger("isRangedBurstShot");
            }
            bulletCount--;
        }
		else
		{
            WeaponController.Animator.SetTrigger("isCancelAction");
            base.DoAction(fov);
        }
    }
}

