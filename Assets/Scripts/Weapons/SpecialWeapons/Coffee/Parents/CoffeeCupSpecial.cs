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
public abstract class CoffeeCupSpecial : AbstractSpecial
{
	[SerializeField]
	protected GameObject grenade;
	[SerializeField]
	protected float explodeRadius;
	[SerializeField]
	protected int explodeRadiusAdder = 2;
	[SerializeField]
	protected int damageAdder = 5;
	private bool isHit;

	public override void ToggleAim(bool isActive)
	{
		if (!SpecialController.ThrowAim.gameObject.activeSelf && isActive)
		{
			SpecialController.ThrowAim.gameObject.SetActive(isActive);
			switch (Charges)
			{
				case 1:
				case 2:
					SpecialController.ThrowAim.SetExplosionSize(explodeRadius * 2);
					break;
				case 3:
					SpecialController.ThrowAim.SetExplosionSize((explodeRadius + explodeRadiusAdder) * 2);
					break;
			}
		}
		else if (SpecialController.ThrowAim.gameObject.activeSelf && !isActive)
		{
			SpecialController.ThrowAim.GetComponent<LineRenderer>().positionCount = 0;
			SpecialController.ThrowAim.DeActivate();
		}
	}

	public override void StartAttack()
	{
		SpecialController.Animator.SetTrigger("isStartSpecialBombard");
	}
	public override void Attack()
	{
		ActionPower = Charges;
		Charges = 0;
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
	public override void EndTurnEffects()
	{
		isHit = false;
	}
	public override void TakeDamageEffect()
	{
		isHit = true;
	}
}
