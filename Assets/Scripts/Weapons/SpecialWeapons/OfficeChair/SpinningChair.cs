using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The spin version of Vickys officeChair (specialWeapon)
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class SpinningChair : AbstractSpecial
{
	[SerializeField]
	private float viewDistance = 2f;
	[SerializeField]
	private float viewAngle = 360f;
	[SerializeField]
	private float damageAdder = 5f;
	[SerializeField]
	private float hitForceMultiplier = 20f;
	[SerializeField]
	private float healAmount = 5f;


	public override void SetFOVSize()
	{
		SpecialController.FOV.ViewAngle = viewAngle;
		SpecialController.FOV.ViewRadius = viewDistance;
	}

	public override void ToggleAim(bool isActive)
	{
		SpecialController.FOVVisualization.SetActive(isActive);
	}

	public override void StartAttack()
	{
		SpecialController.Animator.SetTrigger("isStartSpecialSpin");
	}
	public override void Attack()
	{
		SpecialController.Animator.SetTrigger("isSpecialSpin");
	}

	public override void StartTurnEffect()
	{
		base.AddCharge();
	}
	public override void GiveRegularDamageEffect()
	{
		base.AddCharge();
	}

	public override void DoSpecialAction()
	{
		int nrOfTargets = SpecialController.FOV.VisibleTargets.Count;

		if(Charges == MaxCharges)
		{
			if (nrOfTargets > 0)
			{
				DoDamage();
			}

			Effects.Heal(HolderAgent, healAmount * 2);

			if (nrOfTargets < 2)
			{
				Charges = 0;
			}
		}
		else
		{
			if (nrOfTargets > 0)
			{
				DoDamage();

				if (nrOfTargets > 1)
				{
					if(Charges == 1)
					{
						Effects.Heal(HolderAgent, healAmount);
					}
					else
					{
						Effects.Heal(HolderAgent, healAmount * 2);
					}
				}
			}
			Charges = 0;
		}
	}

	private void DoDamage()
	{
		foreach (GameObject target in SpecialController.FOV.VisibleTargets)
		{
			if (Charges == 0)
			{
				Effects.Damage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
			}
			else
			{
				Effects.Damage(target, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
			}
			Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * Charges)));
		}
	}
}
