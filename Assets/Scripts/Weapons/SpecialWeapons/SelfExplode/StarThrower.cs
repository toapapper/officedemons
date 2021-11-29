using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The supporting version of Susans specialWeapon
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class StarThrower : AbstractSpecial
{
	[SerializeField]
	private float viewDistance = 2f;
	[SerializeField]
	private float viewAngle = 360f;
	[SerializeField]
	private float distanceMultiplier = 1f;
	[SerializeField]
	private GameObject particleEffect;


	public override void SetFOVSize()
	{
		SpecialController.FOV.ViewAngle = viewAngle;
		SpecialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
	}

	public override void ToggleAim(bool isActive)
	{
		SpecialController.FOVVisualization.SetActive(isActive);
	}

	public override void StartAttack()
	{
		SpecialController.Animator.SetTrigger("isStartSpecialSelfExplode");
	}
	public override void Attack()
	{
		SpecialController.Animator.SetTrigger("isSpecialSelfExplode");
	}

	public override void StartTurnEffect()
	{
		AddCharge();

		int targets = SpecialController.FOV.VisibleTargets.Count;
		if (targets > 0)
		{
			//TODO DamageBoost buff effect?
			foreach (GameObject target in SpecialController.FOV.VisibleTargets)
			{
				Effects.ApplyStatusEffect(target, StatusEffectType.DamageBoost);
				base.AddCharge();
			}
			SpecialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
		}
		
		if (Charges == MaxCharges)
		{
			Attack();
		}
	}

	protected override void AddCharge()
	{
		base.AddCharge();
		SpecialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
	}

	public override void DoSpecialAction()
	{
		Instantiate(particleEffect, transform.position, transform.rotation);

		if (SpecialController.FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in SpecialController.FOV.VisibleTargets)
			{
				if(target.tag == "Player")
				{
					Effects.Heal(target, Damage);
				}
			}
			if (Charges == MaxCharges)
			{
				Effects.Heal(HolderAgent, Damage);
			}
		}
		Charges = 0;
		SpecialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
		base.DoSpecialAction();
	}
}
