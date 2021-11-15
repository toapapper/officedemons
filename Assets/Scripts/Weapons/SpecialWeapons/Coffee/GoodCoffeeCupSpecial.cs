using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodCoffeeCupSpecial : CoffeeCupSpecial
{
	[SerializeField]
	private int damageAdder = 20;
	[SerializeField]
	private int aoeAdder = 3;
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
			grenade.GetComponent<GoodCoffee>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
				explodeRadius, /*HitForce,*/ Damage, buffEffects);
		}
		else if (Charges == 1)
		{
			grenade.GetComponent<GoodCoffee>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
			explodeRadius, /*HitForce,*/ Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), buffEffects);
		}
		else
		{
			grenade.GetComponent<GoodCoffee>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
			explodeRadius + aoeAdder, /*HitForce,*/ (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), buffEffects);
		}
		Charges = 0;
	}
}
