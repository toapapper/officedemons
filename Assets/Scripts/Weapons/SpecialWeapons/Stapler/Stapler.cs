﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Tims stapler (specialWeapon)
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14-12-21
public class Stapler : AbstractSpecial
{
	[SerializeField]
	private GameObject weaponMuzzle;
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private float bulletFireForce = 20;
	[SerializeField]
	protected GameObject bullet;
	[SerializeField]
	private int bulletsInBurst = 4;
	private int bulletCount;

	[Tooltip("The maximum amount of degrees away from the direction aimed that the projectile might fly")]
	[SerializeField]
	protected float inaccuracy = 3;

	[SerializeField]
	protected GameObject aimCone;

	/// <summary>
	/// The degrees by which the shot might change direction to either side. The effect of poison is included here
	/// </summary>
	public float Inaccuracy
	{
		get { return Mathf.Clamp(inaccuracy, 0, 89); }
		set { inaccuracy = Mathf.Clamp(value, 0, 89); }
	}

	public override void SetAimColor(Gradient gradient)
	{
		laserAim.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = gradient;
		laserAim.SetActive(false);
		aimCone.GetComponentInChildren<MeshRenderer>().material.color = gradient.colorKeys[0].color;
	}

	public override void ToggleAim(bool isActive)
	{
		laserAim.SetActive(isActive);
		UpdateAimCone();
		aimCone.SetActive(isActive);
	}
	/// <summary>
	/// The maximum amount of degrees from the aim direction that the shot can deviate. This takes the possibility of being po�soned into account.<br/>
	/// Also updates the size and such of the aimcone.
	/// </summary>
	protected void UpdateAimCone()
	{
		float width = 2 * Mathf.Tan(Inaccuracy * Mathf.Deg2Rad);//the 1,1,1 scale of the cone has length one and width one.
		aimCone.transform.localScale = new Vector3(width, 1, 1);
	}

	public override void StartAttack()
	{
		SpecialController.Animator.SetTrigger("isStartSpecialStapler");
	}
	public override void Attack()
	{
		ActionPower = Charges;
		Charges = 0;
		bulletCount = bulletsInBurst;
		SpecialController.Animator.SetTrigger("isSpecialStapler");
	}

	public override void StartTurnEffect()
	{
		base.AddCharge();
	}
	public override void RevivedEffect()
	{
		Charges = MaxCharges;
	}

	/// <summary>
	/// Returns a randomized direction within the weapons (in)accuracy.
	/// </summary>
	/// <param name="aim"></param>
	/// <returns></returns>
	protected Vector3 GetBulletDirection()
	{
		Vector3 bulletDir = transform.forward;//I rotate this forward vector by a random amount of degrees basically
		float deviation = ((Random.value * 2) - 1) * Inaccuracy * Mathf.Deg2Rad;

		float newX = bulletDir.x * Mathf.Cos(deviation) - bulletDir.z * Mathf.Sin(deviation);
		float newZ = bulletDir.x * Mathf.Sin(deviation) + bulletDir.z * Mathf.Cos(deviation);
		bulletDir = new Vector3(newX, 0, newZ);

		return bulletDir;
	}

	public override void DoSpecialAction()
	{
		if(bulletCount < bulletsInBurst)
		{
			if (bulletCount > 0)
			{
				//if (particleEffect)
				//{
				//	Instantiate(particleEffect, weaponMuzzle.transform.position, weaponMuzzle.transform.rotation * Quaternion.Euler(0, 180, 0));
				//}
				Vector3 direction = GetBulletDirection();
				bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, weaponMuzzle.transform.position, direction, bulletFireForce, HitForce, Damage, this.effects);
				bulletCount--;
			}
			else
			{
				SpecialController.Animator.SetTrigger("isCancelAction");
				ActionPower = 0;
			}			
		}
		else
		{
			//if (particleEffect)
			//{
			//	Instantiate(particleEffect, weaponMuzzle.transform.position, weaponMuzzle.transform.rotation * Quaternion.Euler(0, 180, 0));
			//}
			Vector3 direction = GetBulletDirection();
			bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, weaponMuzzle.transform.position, direction, bulletFireForce, HitForce, Damage, this.effects);
			bulletCount--;

			SpecialController.Animator.SetTrigger("isSpecialStaplerShot");
		}
	}


	private StatusEffectType RandomEffect()
    {
		int rnd = Random.Range(1, StatusEffectType.GetNames(typeof(StatusEffectType)).Length - 1);
		return StatusEffectType.rnd;
    }
}
