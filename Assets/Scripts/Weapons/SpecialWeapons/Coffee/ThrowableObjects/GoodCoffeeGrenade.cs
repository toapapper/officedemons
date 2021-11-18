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
public class GoodCoffeeGrenade : GroundEffectGrenade
{
	private GoodCoffeeGrenade coffeeGrenade;
	[SerializeField]
	private PositiveGroundObject coffeeStain;

	public void CreateGrenade(GameObject thrower, Vector3 position, Vector3 direction, float grenadeThrowForce, float explodeRadius, float grenadeHeal, List<StatusEffectType> effects)
	{
		coffeeGrenade = Instantiate(this, position, Quaternion.LookRotation(direction));
		coffeeGrenade.thrower = thrower;
		coffeeGrenade.FOV.ViewRadius = explodeRadius;
		coffeeGrenade.healthModifyAmount = grenadeHeal;
		coffeeGrenade.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
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
			if(target.tag == "Player" || target.tag == "Enemy")
			{
				Effects.Heal(target, healthModifyAmount);
				foreach (StatusEffectType effect in statusEffects)
				{
					Effects.ApplyStatusEffect(target, effect);
				}
			}
		}
		AddToEffectList(coffeeStain);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.transform.tag == "Ground")
		{
			CreateGroundObject(collision.contacts[0].point);
		}
		else
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, LayerMask.GetMask("Ground")))
			{
				CreateGroundObject(hit.point);
			}
		}
		Explode();
	}
}
