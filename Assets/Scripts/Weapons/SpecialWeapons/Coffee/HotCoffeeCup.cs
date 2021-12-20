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
public class HotCoffeeCup : CoffeeCupSpecial
{
	[Header("Disregard all above")]
	[SerializeField] protected GameObject GrenadeOne;
	[SerializeField] protected GameObject GrenadeTwo;
	[SerializeField] protected GameObject GrenadeThree;


    public override void DoSpecialAction()
	{
		Vector3 velocity = SpecialController.ThrowAim.InitialVelocity;
		GameObject grenade = GrenadeOne;
		if (ActionPower == 2)
			grenade = GrenadeTwo;
		else if (ActionPower == 3)
			grenade = GrenadeThree;

		grenade.GetComponent<HotCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, 1,
				HitForce, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), effects);

		ActionPower = 0;
	}
}
