using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The support grenade version thrown by Devins special weapon, [DEPRECATED AND NOT IN USE]
/// healing players and creating buffing coffeStain on impact
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-19
public class GoodCoffeeGrenade : GroundEffectGrenade
{
	private GoodCoffeeGrenade coffeeGrenade;
	[SerializeField]
	private PositiveGroundObject coffeeStain;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 velocity,
		float explodeRadius, float grenadeHeal, List<StatusEffectType> effects)
	{
		coffeeGrenade = Instantiate(this, position, Quaternion.LookRotation(velocity.normalized));
		coffeeGrenade.thrower = thrower;
		coffeeGrenade.FOV.ViewRadius = explodeRadius;
		coffeeGrenade.healthModifyAmount = grenadeHeal;
		coffeeGrenade.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
		GameManager.Instance.StillCheckList.Add(coffeeGrenade.gameObject);

		coffeeGrenade.statusEffects = effects;
	}

	protected override void CreateGroundObject(Vector3 groundObjectPos)
	{
		coffeeStain.CreateGroundObject(groundObjectPos, FOV.ViewRadius, healthModifyAmount, statusEffects);
	}

	protected override void ImpactAgents()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;
		foreach (GameObject target in targetList)
		{
			if(target.tag == "Player")
			{
				if(target.GetComponent<Attributes>().Health > 0)
				{
					Effects.Heal(target, healthModifyAmount);
					//foreach (StatusEffectType effect in statusEffects)
					//{
					//	Effects.ApplyStatusEffect(target, effect);
					//}
				}
			}
		}
		//AddToEffectList(coffeeStain);
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		AkSoundEngine.PostEvent("Play_Splash", gameObject);
		Explode();
	}
}
