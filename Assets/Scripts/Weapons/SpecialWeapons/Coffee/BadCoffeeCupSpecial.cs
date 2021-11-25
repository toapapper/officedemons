using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The harmful version of Devins specialWeapons
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class BadCoffeeCupSpecial : CoffeeCupSpecial
{
	[SerializeField]
	private int explodeRadiusAdder = 2;
	[SerializeField]
	private int damageAdder = 5;
	

	public override void DoSpecialAction()
	{
		Vector3 forward = HolderAgent.transform.forward;
		forward.y = 0;
		forward.Normalize();
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 direction = (Quaternion.AngleAxis(-SpecialController.ThrowAim.initialAngle, right) * forward).normalized;
		//float throwForce = specialController.ThrowAim.initialVelocity;
		float throwForce = SpecialController.ThrowAim.ThrowForce;


		if (Charges == 0)
		{
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, direction, throwForce,
				explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);
		}
		else if (Charges == 1)
		{
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, direction, throwForce,
			explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), ultiEffects);
		}
		else
		{
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, direction, throwForce,
			explodeRadius + explodeRadiusAdder, HitForce, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), ultiEffects);
		}
		Charges = 0;
		base.DoSpecialAction();
	}
}
