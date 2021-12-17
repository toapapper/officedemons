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

    /*
	 * Tre olika grenades som gör lite olika saker
	 * Vill egentligen att grenades är en egen prefab där alla dens stats finns.
	 * 
	 * Skapa cluster-script som skjuter ut sina egna små granater också som ska finnas på några av granaternas explosioner.
	 * 
	 * Kastar tre granater i tre charges bara är väl det som är annorlunda.
	 */

    private void Update()
    {
        if (ActionPower == 3 && SpecialController.ThrowAim.gameObject.activeSelf)
        {
			//aiming throw och tre charges. Visa tre st aiming-saker här.
		}
	}

    public override void DoSpecialAction()
	{
		Vector3 velocity = SpecialController.ThrowAim.InitialVelocity;

		switch (ActionPower)
		{
			case 1:
				grenade.GetComponent<HotCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius,
				HitForce, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), effects);
				break;
			case 2:
				grenade.GetComponent<HotCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius,
				HitForce, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), ultiEffects);
				break;
			case 3:
				grenade.GetComponent<HotCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius + explodeRadiusAdder,
				HitForce, (Damage + damageAdder) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), ultiEffects);
				break;
		}
		ActionPower = 0;
	}

	protected void LaunchGrenade(GameObject grenade, Vector3 velocity, float damage)
    {
		grenade.GetComponent<HotCoffeeGrenade>().CreateGrenade(HolderAgent, transform.position, velocity, explodeRadius + explodeRadiusAdder,
				HitForce, damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), ultiEffects);
	}
}
