using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperShredder : AbstractSpecial
{
	[SerializeField]
	private float viewDistance = 1f;
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
	}
	public override void TakeDamageEffect()
	{
		AddCharge();
	}
	protected override void AddCharge()
	{
		base.AddCharge();

		specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
		if (Charges == MaxCharges)
		{
			Attack();
		}
	}

	public override void DoSpecialAction()
	{
		if (specialController.FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in specialController.FOV.VisibleTargets)
			{
				if(Charges < MaxCharges)
				{
					Effects.ApplyWeaponEffects(target, effects);
				}
				else
				{
					Effects.Damage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
					Effects.ApplyForce(target, (target.transform.position - specialController.FOV.transform.position).normalized * HitForce);
					Effects.ApplyWeaponEffects(target, ultiEffects);
				}				
			}
		}
		Charges = 0;
		specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
	}
}
