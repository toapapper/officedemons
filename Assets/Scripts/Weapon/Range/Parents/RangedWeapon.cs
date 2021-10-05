using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public abstract class RangedWeapon : AbstractWeapon
{
	private GameObject weaponMuzzle;
	private GameObject laserAim;
	[SerializeField]
	private GameObject bullet;
	private int bulletFireForce;

	protected GameObject WeaponMuzzle
	{
		get { return weaponMuzzle; }
		set { weaponMuzzle = value; }
	}
	protected GameObject LaserAim
	{
		get { return laserAim; }
		set { laserAim = value; }
	}
	public int BulletFireForce
	{
		get { return bulletFireForce; }
		set { bulletFireForce = value; }
	}

	public override void Shoot()
	{
		Vector3 bulletDirection = transform.forward;
		bulletDirection.y = 0;
		bulletDirection.Normalize();

		bullet.GetComponent<Bullet>().CreateBullet(WeaponMuzzle.transform.position, bulletDirection, BulletFireForce, HitForce, Damage);


	}
	public override void ToggleLaserAim(bool isActive, Gradient laserSightMaterial)
	{
		LaserAim.SetActive(isActive);
		if (isActive)
		{
			GetComponentInChildren<LineRenderer>().colorGradient = laserSightMaterial;
		}
	}
	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartRangedSingleShot");
	}
	public override abstract void Attack(Animator animator);
}
