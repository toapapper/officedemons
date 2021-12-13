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
public class TankShotWeapon : RangedWeapon
{
    public override void Attack(Animator animator)
    {
        animator.SetTrigger("isRangedSingleShot");
        base.Attack(animator);
    }

    public override Vector3 GetBulletDirection()
    {
        Vector3 bulletDir = transform.forward;//I rotate this forward vector by a random amount of degrees basically
        float deviation = ((Random.value * 2) - 1) * Inaccuracy * Mathf.Deg2Rad;

        float newX = bulletDir.x * Mathf.Cos(deviation) - bulletDir.z * Mathf.Sin(deviation);
        float newZ = bulletDir.x * Mathf.Sin(deviation) + bulletDir.z * Mathf.Cos(deviation);
        bulletDir = new Vector3(newX, transform.forward.y, newZ);

        return bulletDir;
    }

    public override void DoAction()
    {
        if (particleEffect)
        {
            Instantiate(particleEffect, WeaponMuzzle.transform.position, WeaponMuzzle.transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            CameraShake.Shake(0.1f, 0.1f);
        }
        Vector3 direction = GetBulletDirection();
        bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, WeaponMuzzle.transform.position, direction, BulletFireForce, HitForce, Damage, this.effects);
        base.DoAction();
    }
}
