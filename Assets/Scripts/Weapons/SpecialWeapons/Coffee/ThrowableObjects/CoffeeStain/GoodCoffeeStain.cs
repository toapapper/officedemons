using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodCoffeeStain : CoffeStain
{
    private GoodCoffeeStain coffeeStain;
    private float healAmount;
    List<StatusEffectType> effects;

    public void CreateStain(Vector3 position, float stainRadius, float healAmount, List<StatusEffectType> effects)
    {
        coffeeStain = Instantiate(this, position, Quaternion.Euler(0, 0, 0));
        coffeeStain.transform.localScale = new Vector3(stainRadius, coffeeStain.transform.localScale.y, stainRadius);
        coffeeStain.healAmount = healAmount;
        coffeeStain.effects = effects;
        coffeeStain.durabilityTurns = totalDurabilityTurns;
        coffeeStain.durabilityTime = totalDurabilityTime;
        coffeeStain.agentsOnStain = new List<GameObject>();
        GameManager.Instance.GroundEffectObjects.Add(coffeeStain.gameObject);
    }

    protected override void ApplyEffectsOn(GameObject agent)
	{
        //Effects.Heal(agent, healAmount);
        foreach (StatusEffectType effect in effects)
        {
            Effects.ApplyStatusEffect(agent, effect);
        }
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
