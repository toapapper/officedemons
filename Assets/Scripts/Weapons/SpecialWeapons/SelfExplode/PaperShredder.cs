using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The harmful version of Susans specialWeapon
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class PaperShredder : AbstractSpecial
{
	[SerializeField]
	private float viewDistance = 1f;
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
		AkSoundEngine.PostEvent("SusanScream", gameObject);
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

		SpecialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
		if (Charges == MaxCharges)
		{
			Attack();
		}
	}

	public override void DoSpecialAction()
	{
		Instantiate(particleEffect, transform.position, transform.rotation);
		if(Charges < MaxCharges)
        {
			AkSoundEngine.PostEvent("SusanBurst", gameObject);
        }
        else
        {
			AkSoundEngine.PostEvent("Play_Explosion", gameObject);
        }

		if (specialController.FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in SpecialController.FOV.VisibleTargets)
			{
				if (target.layer != LayerMask.NameToLayer("Destructible"))
				{
					if (Charges < MaxCharges)
					{
						Effects.ApplyWeaponEffects(target, effects);
					}
					else
					{
						Effects.WeaponDamage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
						Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * HitForce);
						Effects.ApplyWeaponEffects(target, ultiEffects);
					}
				}
				else if (Charges >= MaxCharges)
				{
					Effects.Damage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
				}
			}
		}
		Charges = 0;
		SpecialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
		base.DoSpecialAction();
	}
}
