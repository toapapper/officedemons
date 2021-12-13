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
	public override void DoSpecialAction()
	{
		Vector3 velocity = SpecialController.ThrowAim.InitialVelocity;

		switch (ActionPower)
		{
			case 1:
				grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius,
				HitForce, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), effects);
				break;
			case 2:
				grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius,
				HitForce, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), ultiEffects);
				break;
			case 3:
				grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius + explodeRadiusAdder,
				HitForce, (Damage + damageAdder) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), ultiEffects);
				break;
		}
		ActionPower = 0;
	}
}
