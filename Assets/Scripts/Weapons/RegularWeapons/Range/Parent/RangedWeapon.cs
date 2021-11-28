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

// Last Edited: 14/10-28
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

	[Tooltip("The maximum amount of degrees away from the direction aimed that the projectile might fly")]
	[SerializeField]
	protected float inaccuracy = 3;

	[SerializeField]
	protected GameObject AimCone;

	[SerializeField]
	protected GameObject particleEffect;

	protected GameObject WeaponMuzzle
	{
		get { return weaponMuzzle; }
		set { weaponMuzzle = value; }
	}
	protected float BulletFireForce
	{
		get { return bulletFireForce; }
		set { bulletFireForce = value; }
	}


	/// <summary>
	/// The degrees by which the shot might change direction to either side. The effect of poison is included here
	/// </summary>
	public float Inaccuracy
	{
		get
		{
			float modval = 0;
			if (this.HolderAgent != null)
			{
				modval = this.HolderAgent.GetComponent<StatusEffectHandler>().InAccuracyMod;
			}
			return Mathf.Clamp(inaccuracy + modval, 0, 89);
		}
		set { inaccuracy = Mathf.Clamp(value, 0, 89); }
	}

	public override void SetAimGradient(Gradient gradient)
	{
		laserAim.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = gradient;
		laserAim.SetActive(false);
		AimCone.GetComponentInChildren<MeshRenderer>().material.color = gradient.colorKeys[0].color;
	}

	public override void ToggleAim(bool isActive, GameObject FOVView)
	{
		if (Inaccuracy < 1)
		{
			laserAim.SetActive(isActive);
		}
		else
		{
			UpdateAimCone();
			AimCone.SetActive(isActive);
		}
	}

	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartRangedSingleShot");
	}

	public override void Attack(Animator animator)
	{
		base.Attack(animator);
	}


	/// <summary>
	/// The maximum amount of degrees from the aim direction that the shot can deviate. This takes the possibility of being po�soned into account.<br/>
	/// Also updates the size and such of the aimcone.
	/// </summary>
	protected void UpdateAimCone()
	{
		float width = 2 * Mathf.Tan(Inaccuracy * Mathf.Deg2Rad);//the 1,1,1 scale of the cone has length one and width one.
		AimCone.transform.localScale = new Vector3(width, 1, 1);
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

	public override void DoAction(FieldOfView fov)
	{
		GameObject wielder = gameObject.GetComponentInParent<Attributes>().gameObject;
		if (wielder == null)
		{
			return;
		}

		Vector3 direction = GetBulletDirection();

		bullet.GetComponent<Bullet>().CreateBullet(HolderAgent, WeaponMuzzle.transform.position, direction, BulletFireForce, HitForce, Damage/* * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost)*/, this.effects);

		//recoil and slippery-checks

		//Check for recoil recoil deals half the weapondamage and applies the effects
		if (effects.Contains(WeaponEffects.Recoil))
		{
			float rand = Random.value;
			if (rand < RecoilChance)
			{
				Effects.WeaponDamage(wielder, Damage / 2);
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
		if (particleEffect)
		{
			Instantiate(particleEffect, WeaponMuzzle.transform.position, WeaponMuzzle.transform.rotation * Quaternion.Euler(0f, 180f, 0f)/*Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z)*/);

		}
		base.DoAction(fov);
	}
}
