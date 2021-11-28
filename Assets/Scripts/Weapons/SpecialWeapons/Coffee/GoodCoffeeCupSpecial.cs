using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The supporting version of Devins specialWeapons
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class GoodCoffeeCupSpecial : CoffeeCupSpecial
{
	[SerializeField]
	private List<StatusEffectType> buffEffects;


	public override void DoSpecialAction()
	{
		Vector3 velocity = SpecialController.ThrowAim.InitialVelocity;

		switch (Charges)
		{
			case 1:
				grenade.GetComponent<GoodCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity,explodeRadius, 0, buffEffects);
				break;
			case 2:
				grenade.GetComponent<GoodCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity,
			explodeRadius, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), buffEffects);
				break;
			case 3:
				grenade.GetComponent<GoodCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity,explodeRadius + explodeRadiusAdder,
					(Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), buffEffects);

				break;
		}
		Charges = 0;
	}
}
