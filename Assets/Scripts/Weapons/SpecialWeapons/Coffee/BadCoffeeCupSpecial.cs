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
		Vector3 velocity = SpecialController.ThrowAim.InitialVelocity;

		if (Charges == 0)
		{
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity,explodeRadius,
				HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);
		}
		else if (Charges == 1)
		{
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius,
				HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), ultiEffects);
		}
		else
		{
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius + explodeRadiusAdder,
				HitForce, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), ultiEffects);
		}
		Charges = 0;
		base.DoSpecialAction();
	}
}
