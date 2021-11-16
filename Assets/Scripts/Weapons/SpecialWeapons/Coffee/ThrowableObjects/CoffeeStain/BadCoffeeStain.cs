using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The harmful stain left by Devins BadCoffeeGrenade
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class BadCoffeeStain : CoffeStain
{
	private BadCoffeeStain coffeeStain;
    private float damage;
    List<WeaponEffects> effects;

    public void CreateStain(Vector3 position, float stainRadius, float damage, List<WeaponEffects> effects)
    {
        coffeeStain = Instantiate(this, position, Quaternion.Euler(0, 0, 0));
        coffeeStain.transform.localScale = new Vector3(stainRadius, coffeeStain.transform.localScale.y, stainRadius);
        coffeeStain.damage = damage;
        coffeeStain.effects = effects;
        coffeeStain.durabilityTurns = totalDurabilityTurns;
        coffeeStain.durabilityTime = totalDurabilityTime;
        coffeeStain.agentsOnStain = new List<GameObject>();
        GameManager.Instance.GroundEffectObjects.Add(coffeeStain.gameObject);
    }

    protected override void ApplyEffectsOn(GameObject agent)
    {
		//Effects.Damage(agent, damage);
		Effects.ApplyWeaponEffects(agent, effects);
	}

    protected override void OnTriggerEnter(Collider other)
	{
        if (!agentsOnStain.Contains(other.gameObject))
        {
            if (other.tag == "Player" || other.tag == "Enemy")
            {
				ApplyEffectsOn(other.gameObject);
				base.OnTriggerEnter(other);
            }
        }
    }
	
}
