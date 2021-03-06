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
	[SerializeField] private List<GameObject> particleEffects;
	private bool changedFOV;
	private bool refundable;
	private bool isKillEffect;
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
            if (Charges > MaxCharges)
            {
				Charges = MaxCharges;
            }
			switch (Charges)
			{
				case 1:
					ActionPower = 1;
					break;
				case 2:
					ActionPower = 1.5f;
					break;
				case 3:
					ActionPower = 2;
					break;
				default:
					ActionPower = 0;
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

    public override void KillEffect()
    {
        base.KillEffect();
		Debug.Log("WE KILLED SOMETING");
        if (refundable)
        {
			Debug.Log("it was refundable");
			isKillEffect = true;
			//gameObject.transform.parent.transform.GetComponentInChildren<StatusEffectHandler>().ApplyEffect(StatusEffectType.damage_boost, HolderAgent);
		}
	}

    public override void DoSpecialAction()
	{
		int nrOfTargets = specialController.FOV.VisibleTargets.Count;
		AkSoundEngine.PostEvent("VickySlide", gameObject);
		AkSoundEngine.PostEvent("SusanBurst", gameObject);
        switch (Charges)
        {
            case 1:
                Instantiate(particleEffects[0], transform.position, Quaternion.Euler(-90, 0, 0));
                break;
            case 2:
                Instantiate(particleEffects[1], transform.position, Quaternion.Euler(-90, 0, 0));
                break;
            case 3:
                Instantiate(particleEffects[2], transform.position, Quaternion.Euler(-90, 0, 0));
                break;
            default:
                break;
        }
		if (Charges >= MaxCharges)
		{
			refundable = true;
			if (nrOfTargets > 0)
			{
				DoDamage();
			}

			Effects.Heal(HolderAgent, healAmount * ActionPower);
		}
		else
		{
			refundable = false;
			if (nrOfTargets > 0)
			{
				DoDamage();

				if (nrOfTargets > 1)
				{
					Effects.Heal(HolderAgent, healAmount * ActionPower);
				}
			}
			Charges = 0;
		}
		StartCoroutine(CountdownTime(0.5f));
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
						Effects.WeaponDamage(target, Damage * ActionPower, HolderAgent);
						break;
					case 2:
						Effects.WeaponDamage(target, (Damage + damageAdder) * ActionPower, HolderAgent);
						break;
					case 3:
						Effects.WeaponDamage(target, (Damage + damageAdder) * ActionPower, HolderAgent);
						Effects.ApplyWeaponEffects(target, HolderAgent, effects);
						break;

				}
				Effects.ApplyForce(target, (target.transform.position - SpecialController.FOV.transform.position).normalized * (HitForce + (hitForceMultiplier * ActionPower)));
			}
			else
			{
				switch (Charges)
				{
					case 1:
						Effects.Damage(target, Damage * ActionPower);
						break;
					case 2:
						Effects.Damage(target, (Damage + damageAdder) * ActionPower);
						break;
					case 3:
						Effects.Damage(target, (Damage + damageAdder) * ActionPower);
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
		foreach (TrailRenderer trail in trails)
		{
			trail.enabled = false;
		}
        if (!refundable)
        {
			Charges = 0;
		}
		else if (refundable)
		{
			Charges = 0;
            if (isKillEffect)
            {
				Charges = MaxCharges;
				isKillEffect = false;
            }
			refundable = false;
		}
	}
}
