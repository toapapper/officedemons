using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCoffeeCupSpecial : CoffeeCupSpecial
{
	[SerializeField]
	private int explodeRadiusAdder = 2;
	[SerializeField]
	private int damageAdder = 5;
	

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
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
				explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);
		}
		else if (Charges == 1)
		{
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
			explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), ultiEffects);
		}
		else
		{
			grenade.GetComponent<BadCoffeeGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
			explodeRadius + explodeRadiusAdder, HitForce, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), ultiEffects);
		}
		Charges = 0;
	}
}
