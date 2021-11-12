using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarThrower : AbstractSpecial
{
	[SerializeField]
	private float viewDistance = 2f;
	[SerializeField]
	private float viewAngle = 360f;
	[SerializeField]
	private float distanceMultiplier = 1f;



	public override void SetFOVSize()
	{
		specialController.FOV.ViewAngle = viewAngle;
		specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
	}

	public override void ToggleAim(bool isActive)
	{
		specialController.FOVVisualization.SetActive(isActive);
	}

	public override void StartAttack()
	{
		specialController.Animator.SetTrigger("isStartSpecialSelfExplode");
	}
	public override void Attack()
	{
		specialController.Animator.SetTrigger("isSpecialSelfExplode");
	}

	public override void StartTurnEffect()
	{
		AddCharge();

		int targets = specialController.FOV.VisibleTargets.Count;
		if (targets > 0)
		{
			//TODO DamageBoost buff effect?
			foreach (GameObject target in specialController.FOV.VisibleTargets)
			{
				Effects.ApplyStatusEffect(target, StatusEffectType.DamageBoost);
				base.AddCharge();
			}
			specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
		}
		
		if (Charges == MaxCharges)
		{
			Attack();
		}
	}

	protected override void AddCharge()
	{
		base.AddCharge();
		specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
	}

	public override void DoSpecialAction()
	{
		if (specialController.FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in specialController.FOV.VisibleTargets)
			{
				if(target.tag == "Player")
				{
					Effects.Heal(target, Damage);
				}
			}
			if (Charges == MaxCharges)
			{
				Effects.Heal(holderAgent, Damage);
			}
		}
		Charges = 0;
		specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
	}
}
