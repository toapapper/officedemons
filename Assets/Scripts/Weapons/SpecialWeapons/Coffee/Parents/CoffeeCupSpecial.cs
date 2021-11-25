using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The abstract version of Devins specialWeapons
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class CoffeeCupSpecial : AbstractSpecial
{
	[SerializeField]
	protected GameObject grenade;
	[SerializeField]
	protected float explodeRadius;
	private bool isHit;

	public override void ToggleAim(bool isActive)
	{
		if (!isActive)
		{
			SpecialController.ThrowAim.GetComponent<LineRenderer>().positionCount = 0;
			SpecialController.ThrowAim.DeActivate();
		}
		else
		{
			SpecialController.ThrowAim.gameObject.SetActive(isActive);
			SpecialController.ThrowAim.SetExplosionSize(explodeRadius * 2);
		}
	}

	public override void StartAttack()
	{
		SpecialController.Animator.SetTrigger("isStartSpecialBombard");
	}
	public override void Attack()
	{
		SpecialController.Animator.SetTrigger("isSpecialBombard");
	}

	public override void StartTurnEffect()
	{
		if (!isHit)
		{
			base.AddCharge();
		}
		else
		{
			isHit = false;
		}

		base.AddCharge();
	}
	public override void TakeDamageEffect()
	{
		isHit = true;
	}

	public override void DoSpecialAction()
	{
		SpecialController.ToggleAimView(false);
	}
}
