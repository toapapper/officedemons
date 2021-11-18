using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveGroundObject : GroundEffectObject
{
	private PositiveGroundObject groundObject;
	protected float healAmount;
	protected List<StatusEffectType> effects;

	public void CreateGroundObject(Vector3 position, float stainRadius, float healAmount, List<StatusEffectType> effects)
	{
		groundObject = Instantiate(this, position, Quaternion.Euler(0, 0, 0));
		groundObject.transform.localScale = new Vector3(stainRadius, groundObject.transform.localScale.y, stainRadius);
		groundObject.healAmount = healAmount;
		groundObject.effects = effects;
		groundObject.durabilityTurns = totalDurabilityTurns;
		groundObject.durabilityTime = totalDurabilityTime;
		groundObject.agentsOnGroundEffect = new List<GameObject>();
		GameManager.Instance.GroundEffectObjects.Add(groundObject.gameObject);
	}

	protected override void ApplyEffectsOn(GameObject agent)
	{
		Effects.Heal(agent, healAmount);
		foreach (StatusEffectType effect in effects)
		{
			Effects.ApplyStatusEffect(agent, effect);
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger)
		{
			if (other.tag == "Player" || other.tag == "Enemy")
			{
				if (other.GetComponent<Attributes>().Health > 0)
				{
					ApplyEffectsOn(other.gameObject);
					base.OnTriggerEnter(other);
				}
			}
		}
	}
}
