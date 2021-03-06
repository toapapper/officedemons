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
    [SerializeField] ParticleSystem casings;
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

    public override void DoAction(/*FieldOfView fov*/)
    {
        casings.Emit(1);
        if (bulletCount > 0)
		{
            if (particleEffect)
            {
                Instantiate(particleEffect, WeaponMuzzle.transform.position, WeaponMuzzle.transform.rotation * Quaternion.Euler(0, 0, 0));
                CameraShake.Shake(0.1f, 0.1f);
            }
            Vector3 direction = GetBulletDirection();
            float dmg = Damage;

            bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, WeaponMuzzle.transform.position, direction, BulletFireForce, HitForce, dmg, Utilities.ListDictionaryKeys(effects));
            if (bulletCount == bulletsInBurst)
            {
                WeaponController.Animator.SetTrigger("isRangedBurstShot");
            }

            Effects.ApplyForce(HolderAgent, HolderAgent.transform.forward * -1 * recoilPower);


            bulletCount--;
        }
		else
		{
            WeaponController.Animator.SetTrigger("isCancelAction");
            base.DoAction(/*fov*/);
        }
    }
}

