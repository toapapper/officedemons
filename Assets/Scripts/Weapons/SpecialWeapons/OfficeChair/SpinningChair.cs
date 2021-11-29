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
		specialController.FOV.ViewAngle = viewAngle;
		specialController.FOV.ViewRadius = viewDistance;
	}

	public override void ToggleAim(bool isActive)
	{
		specialController.FOVVisualization.SetActive(isActive);
	}

	public override void StartAttack()
	{
		specialController.Animator.SetTrigger("isStartSpecialSpin");
	}
	public override void Attack()
	{
		specialController.Animator.SetTrigger("isSpecialSpin");
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
		int nrOfTargets = specialController.FOV.VisibleTargets.Count;
		AkSoundEngine.PostEvent("VickySlide", gameObject);
		AkSoundEngine.PostEvent("SusanBurst", gameObject);
		if (Charges == MaxCharges)
		{
			if (nrOfTargets > 0)
			{
				DoDamage();
			}

			Effects.Heal(holderAgent, healAmount * 2);

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
						Effects.Heal(holderAgent, healAmount);
					}
					else
					{
						Effects.Heal(holderAgent, healAmount * 2);
					}
				}
			}
			Charges = 0;
		}
	}

	private void DoDamage()
	{
		foreach (GameObject target in specialController.FOV.VisibleTargets)
		{
			if (Charges == 0)
			{
				Effects.Damage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
			}
			else
			{
				Effects.Damage(target, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
			}
			Effects.ApplyForce(target, (target.transform.position - specialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * Charges)));
		}
	}
}
