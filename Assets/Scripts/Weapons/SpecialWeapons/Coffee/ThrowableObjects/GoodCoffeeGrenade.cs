using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The support grenade version thrown by Devins special weapon
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-17
public class GoodCoffeeGrenade : CoffeeGrenade
{
	private GoodCoffeeGrenade coffeeGrenade;
	[SerializeField]
	private GoodCoffeeStain coffeeStain;
	protected List<StatusEffectType> effects;
	protected float grenadeHeal;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce, float explodeRadius, float grenadeHeal, List<StatusEffectType> effects)
	{
		coffeeGrenade = Instantiate(this, position, Quaternion.LookRotation(direction));
		coffeeGrenade.thrower = thrower;
		coffeeGrenade.GetComponent<FieldOfView>().ViewRadius = explodeRadius;
		coffeeGrenade.grenadeHeal = grenadeHeal;
		coffeeGrenade.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
		GameManager.Instance.StillCheckList.Add(coffeeGrenade.gameObject);

		coffeeGrenade.effects = effects;
	}

	protected override void Explode()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;
		foreach (GameObject target in targetList)
		{
			if(target.tag == "Player" || target.tag == "Enemy")
			{
				coffeeStain.agentsOnStain.Add(target);

				Effects.Heal(target, grenadeHeal);
				foreach (StatusEffectType effect in effects)
				{
					Effects.ApplyStatusEffect(target, effect);
				}
			}
		}
	}

	protected override void CreateStain(Vector3 stainPosition)
	{
		coffeeStain.CreateStain(stainPosition, GetComponent<FieldOfView>().ViewRadius, grenadeHeal, effects);
	}
}
