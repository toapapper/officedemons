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
	private float hitForceMultiplier = 60f;
	[SerializeField]
	private float healAmount = 10f;
	[SerializeField]
	private List<TrailRenderer> trails;
	[SerializeField]
	private GameObject particleEffect;
	private bool changedFOV;
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
		if (!changedFOV)
		{
			switch (Charges)
			{
				case 1:
					ActionPower = 1;
					break;
				case 2:
					ActionPower = 2.5f;
					break;
				case 3:
					ActionPower = 4;
					break;
				default:
					break;
			}
			specialController.FOV.ViewRadius *= ActionPower;
		}
		SpecialController.FOVVisualization.SetActive(isActive);
		changedFOV = true;

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
		Charges = 0;
		changedFOV = false;
		SpecialController.Animator.SetTrigger("isSpecialSpin");
	}

	public override void StartTurnEffect()
	{
		SpecialController.FOV.ViewRadius = viewDistance;
		SpecialController.FOV.ViewAngle = viewAngle;
		//base.AddCharge();
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
		Instantiate(particleEffect, transform.position, Quaternion.Euler(-90,0,0));
		Debug.LogError(particleEffect);
		if (Charges == MaxCharges)
		{
			if (nrOfTargets > 0)
			{
				DoDamage();
				if (nrOfTargets >= MaxCharges)
				{
					Charges = MaxCharges;
				}
			}

			Effects.Heal(HolderAgent, healAmount * ActionPower);
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
						Effects.Heal(HolderAgent, healAmount * ActionPower);
					}
					else
					{
						Effects.Heal(HolderAgent, healAmount * ActionPower);
					}
				}
			}
		}
		StartCoroutine(CountdownTime(0.5f));
		ActionPower = 0;
		changedFOV = false;
		        SpecialController.FOV.ViewRadius = viewDistance;
        SpecialController.FOV.ViewAngle = viewAngle;
	}

	private void DoDamage()
	{
		foreach (GameObject target in SpecialController.FOV.VisibleTargets)
		{
			if (target.layer != LayerMask.NameToLayer("Destructible"))
			{
				switch (Charges)
				{
					case 1:
						Effects.WeaponDamage(target, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost) * ActionPower, HolderAgent);
						break;
					case 2:
						Effects.WeaponDamage(target, (Damage + damageAdder) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost) * ActionPower, HolderAgent);
						break;
					case 3:
						Effects.WeaponDamage(target, (Damage + damageAdder) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost) * ActionPower, HolderAgent);
						Effects.ApplyWeaponEffects(target, effects);
						break;

				}
				Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * ActionPower)));
			}
			else
			{
				switch (Charges)
				{
					case 1:
						Effects.Damage(target, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost) * ActionPower);
						break;
					case 2:
						Effects.Damage(target, (Damage + damageAdder) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost) * ActionPower);
						break;
					case 3:
						Effects.Damage(target, (Damage + damageAdder) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost) * ActionPower);
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
