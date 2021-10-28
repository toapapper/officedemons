using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// Methods connected to all ranged weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public abstract class RangedWeapon : AbstractWeapon
{
	[SerializeField]
	private GameObject weaponMuzzle;
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private float bulletFireForce = 20;
	[SerializeField]
	protected GameObject bullet;

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
	public float BulletFireForce
	{
		get { return bulletFireForce; }
		set { bulletFireForce = value; }
	}

	public override void SetAimGradient(Gradient gradient)
	{
		LaserAim.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = gradient;
		LaserAim.SetActive(false);
	}

	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		LaserAim.SetActive(isActive);
	}

	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartRangedSingleShot");
	}
	public override void Attack(Animator animator)
	{
		base.Attack(animator);
	}

	public override void DoAction(FieldOfView fov)
	{
		Vector3 direction = transform.forward;
		direction.y = 0;
		direction.Normalize();

		bullet.GetComponent<Bullet>().CreateBullet(WeaponMuzzle.transform.position, direction, BulletFireForce, HitForce, Damage);

		base.DoAction(fov);
	}
}
