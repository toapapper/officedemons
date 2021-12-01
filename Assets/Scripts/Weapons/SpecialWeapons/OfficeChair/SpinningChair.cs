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
	[SerializeField]
	private List<TrailRenderer> trails;

	private void Start()
	{
		foreach (TrailRenderer trail in trails)
		{
			trail.enabled = false;
		}
	}

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
		foreach (TrailRenderer trail in trails)
		{
			trail.enabled = true;
		}
		ActionPower = Charges;
		Charges = 0;
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
		int nrOfTargets = specialController.FOV.VisibleTargets.Count;
		AkSoundEngine.PostEvent("VickySlide", gameObject);
		AkSoundEngine.PostEvent("SusanBurst", gameObject);
		if (ActionPower == MaxCharges)
		{
			if (nrOfTargets > 0)
			{
				DoDamage();
				if (nrOfTargets >= MaxCharges)
				{
					Charges = MaxCharges;
				}
			}

			Effects.Heal(HolderAgent, healAmount * 2);
		}
		else
		{
			if (nrOfTargets > 0)
			{
				DoDamage();

				if (nrOfTargets > 1)
				{
					if(ActionPower == 1)
					{
						Effects.Heal(HolderAgent, healAmount);
					}
					else
					{
						Effects.Heal(HolderAgent, healAmount * 2);
					}
				}
			}
		}
		StartCoroutine(CountdownTime(0.5f));
	}

	private void DoDamage()
	{
		foreach (GameObject target in SpecialController.FOV.VisibleTargets)
		{
			if (target.layer != LayerMask.NameToLayer("Destructible"))
			{
				switch (ActionPower)
				{
					case 1:
						Effects.WeaponDamage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
						break;
					case 2:
						Effects.WeaponDamage(target, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
						break;
					case 3:
						Effects.WeaponDamage(target, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
						Effects.ApplyWeaponEffects(target, effects);
						break;

				}
				Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * ActionPower)));
			}
			else
			{
				switch (ActionPower)
				{
					case 1:
						Effects.Damage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
						break;
					case 2:
						Effects.Damage(target, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
						break;
					case 3:
						Effects.Damage(target, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
						break;

				}
			}
		}
	}

	private IEnumerator CountdownTime(float time)
	{
		yield return new WaitForSeconds(time);
		EndSpecial();
	}

	private void EndSpecial()
	{
		ActionPower = 0;
		foreach (TrailRenderer trail in trails)
		{
			trail.enabled = false;
		}
	}
}
