using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all bombard weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-30
public class BombardWeapon : AbstractWeapon
{
	[SerializeField]
	private GameObject grenade;
	[SerializeField]
	private float grenadeThrowForce = 10;
	[SerializeField]
	private bool noBouncing;

	public float GrenadeThrowForce
	{
		get { return grenadeThrowForce; }
		set { grenadeThrowForce = value; }
	}

	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		if (!isActive)
		{
			throwAim.GetComponent<LineRenderer>().positionCount = 0;
		}
		throwAim.SetActive(isActive);
		throwAim.GetComponent<ThrowAim>().NoBounceing = noBouncing;
	}

	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartBombard");
	}
	public override void Attack(Animator animator)
	{
		base.Attack(animator);
		animator.SetTrigger("isBombard");
	}

	public override void DoAction(FieldOfView fov)
	{
		GameObject wielder = gameObject.GetComponentInParent<Attributes>().gameObject;
		if (wielder == null)
		{
			return;
		}

		Vector3 forward = transform.forward;
		forward.y = 0;
		forward.Normalize();
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 direction = (Quaternion.AngleAxis(-GetComponentInParent<WeaponHand>().ThrowAim.initialAngle, right) * forward).normalized;
		float throwForce = GetComponentInParent<WeaponHand>().ThrowAim.initialVelocity;
		Debug.Log(effects);
		grenade.GetComponent<GrenadeObject>().CreateGrenade(transform.position, direction, throwForce, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);

		//recoil and slippery-checks
		//deals half the weapondamage and applies the effects
		if (effects.Contains(WeaponEffects.Recoil))
		{
			float rand = Random.value;
			if (rand < RecoilChance)
			{
				Effects.Damage(wielder, Damage / 2);
				Effects.ApplyForce(wielder, (wielder.transform.forward * -1 * HitForce));
				Effects.ApplyWeaponEffects(wielder, effects);			}
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
