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

	[Tooltip("100 means pinpoint accurate. 0 means the shot might go as far as 90 degrees off from where you aim")] 
	[SerializeField]
	protected float accuracy = 100;

	[SerializeField]
	protected GameObject AimCone;
	[SerializeField]
	protected Transform AimPoint;//Point used to not need the complicated rotation of vector maths....


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

	/// <summary>
	/// The accuracy of the weapon. 100 means totally accurate, 0 means the shot might go as far as 90 degrees to the side.
	/// </summary>
	public float Accuracy
    {
        get { return Mathf.Clamp(accuracy, 0, 100); }//the clamps are for safety
		set { accuracy = Mathf.Clamp(value, 0, 100); }
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


	/// <summary>
	/// The maximum amount of degrees from the aim direction that the shot can deviate. This takes the possibility of being poísoned into account.<br/>
	/// Also updates the size and such of the aimcone.
	/// </summary>
	protected float MaxShotDeviation()
    {
		float accuracyVal = Mathf.Clamp(Accuracy - (this.GetComponentInParent<StatusEffectHandler>().ActiveEffects.ContainsKey(StatusEffectType.Poison) ? StatusEffectHandler.posionAccuracyLoss : 0), 0, 100);
		accuracyVal = ((100 - accuracyVal) / 100) * 90;

		//Update the scales of aimCone.
		//räkna ut bredd om längd är ett.
		float width = 2 * Mathf.Tan(accuracyVal * Mathf.Deg2Rad);
		AimCone.transform.localScale = new Vector3(width, 1, 1);

		return accuracyVal;
    }

	/// <summary>
	/// Returns a randomized direction within the weapons accuracy.
	/// </summary>
	/// <param name="aim"></param>
	/// <returns></returns>
	protected Vector3 GetBulletDirection()
    {
		//float mod = this.GetComponentInParent<StatusEffectHandler>().ActiveEffects.ContainsKey(StatusEffectType.Poison) ? StatusEffectHandler.posionAccuracyLoss : 0;
		//float deviation = Random.value * MaxShotDeviation();
		//float direction = Random.value * 360;

		//Vector3 position = new Vector3(1, 0, 0);

		////rotate the vector to point in the random direction. z axis ignored
		//position = new Vector3(position.x * Mathf.Cos(direction * Mathf.Deg2Rad), position.x * Mathf.Sin(direction * Mathf.Deg2Rad), 0);

		////Now calculate how long it should be in the z axis and scale the one already existing.
		//float length = Mathf.Cos(deviation * Mathf.Deg2Rad);
		//position *= Mathf.Sin(deviation * Mathf.Deg2Rad);
		//position.z = length;

		//this.AimPoint.localPosition = position;//setting its localposition to this rotates it automatically to the space of the gun

		//new simpler in 2d...:

		Vector3 bulletDir = transform.forward;//rotate the forward vector by a random amount of degrees.
		float deviation = ((Random.value * 2) - 1) * MaxShotDeviation() * Mathf.Deg2Rad;

		float newX = bulletDir.x * Mathf.Cos(deviation) - bulletDir.z * Mathf.Sin(deviation);
		float newZ = bulletDir.x * Mathf.Sin(deviation) + bulletDir.z * Mathf.Cos(deviation);
		bulletDir = new Vector3(newX, 0, newZ);

		return bulletDir;//Then return its direction from the gun, in global space
    }

	public override void DoAction(FieldOfView fov)
	{
		GameObject wielder = gameObject.GetComponentInParent<Attributes>().gameObject;
		if (wielder == null)
		{
			return;
		}

		//Vector3 direction = transform.forward;
		//direction.y = 0;
		//direction.Normalize();

		Vector3 direction = GetBulletDirection();

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

		base.DoAction(fov);
	}
}
