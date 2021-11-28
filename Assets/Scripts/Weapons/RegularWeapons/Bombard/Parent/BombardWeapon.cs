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
	protected GameObject grenade;
	[SerializeField]
	private float grenadeThrowForce = 10;
	[SerializeField]
	protected float explodeRadius = 2;
	[SerializeField]
	private bool noBouncing;


	public float GrenadeThrowForce
	{
		get { return grenadeThrowForce; }
		set { grenadeThrowForce = value; }
	}

	public override void ToggleAim(bool isActive, GameObject FOVView)
	{	
		if (!isActive)
		{
			WeaponController.ThrowAim.GetComponent<LineRenderer>().positionCount = 0;
			WeaponController.ThrowAim.DeActivate();
		}
		else
		{
			WeaponController.ThrowAim.gameObject.SetActive(isActive);
			WeaponController.ThrowAim.NoBounceing = noBouncing;
			WeaponController.ThrowAim.SetExplosionSize(explodeRadius * 2);
		}
		
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

		//recoil and slippery-checks
		//deals half the weapondamage and applies the effects
		if (effects.Contains(WeaponEffects.Recoil))
		{
			float rand = Random.value;
			if (rand < RecoilChance)
			{
				Effects.WeaponDamage(wielder, Damage / 2);
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
