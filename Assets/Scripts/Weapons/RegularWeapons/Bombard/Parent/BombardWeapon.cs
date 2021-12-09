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
		if (!WeaponController.ThrowAim.gameObject.activeSelf && isActive)
		{
			WeaponController.ThrowAim.gameObject.SetActive(isActive);
			WeaponController.ThrowAim.NoBounceing = noBouncing;
			WeaponController.ThrowAim.SetExplosionSize(explodeRadius * 2);
		}
		else if (WeaponController.ThrowAim.gameObject.activeSelf && !isActive)
		{
			WeaponController.ThrowAim.GetComponent<LineRenderer>().positionCount = 0;
			WeaponController.ThrowAim.DeActivate();
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

}
