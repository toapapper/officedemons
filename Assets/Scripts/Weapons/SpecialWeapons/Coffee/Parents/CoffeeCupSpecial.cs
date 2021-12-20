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

	[SerializeField] protected float explodeRadius1 = 1;
	[SerializeField] protected float explodeRadius2 = 1.5f;
	[SerializeField] protected float explodeRadius3 = 1.8f;

	private bool isHit;

	public override void ToggleAim(bool isActive)
	{
		float explodeRadius = explodeRadius1;

		if (Charges == 2)
			explodeRadius = explodeRadius2;
		else if (Charges == 3)
			explodeRadius = explodeRadius3;

		if (!SpecialController.ThrowAim.gameObject.activeSelf && isActive)
		{
			SpecialController.ThrowAim.gameObject.SetActive(isActive);
			SpecialController.ThrowAim.SetExplosionSize(explodeRadius * 2);
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
			//Bortkommenterad för att se om det känns bättre. Känns lite op och konstigt just nu.
			//base.AddCharge();
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
