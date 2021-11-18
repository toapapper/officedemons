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
	private float damageAdder = 20f;
	[SerializeField]
	private float explodeRadiusAdder = 1f;
	[SerializeField]
	private List<StatusEffectType> buffEffects;


	public override void DoSpecialAction()
	{
		Vector3 forward = holderAgent.transform.forward;
		forward.y = 0;
		forward.Normalize();
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 direction = (Quaternion.AngleAxis(-specialController.ThrowAim.initialAngle, right) * forward).normalized;
		float throwForce = specialController.ThrowAim.initialVelocity;


		if (Charges == 0)
		{
			grenade.GetComponent<GoodCoffeeGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
				explodeRadius, 0, buffEffects);
		}
		else if (Charges == 1)
		{
			grenade.GetComponent<GoodCoffeeGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
			explodeRadius, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), buffEffects);
		}
		else
		{
			grenade.GetComponent<GoodCoffeeGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
			explodeRadius + explodeRadiusAdder, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), buffEffects);
		}
		Charges = 0;
	}
}
