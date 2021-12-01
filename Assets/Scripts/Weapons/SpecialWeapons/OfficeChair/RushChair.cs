using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The rush version of Vickys officeChair (specialWeapon)
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public class RushChair : AbstractSpecial
{
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private float rushForce = 100f;
	[SerializeField]
	private float rushForceAdder = 50f;
	[SerializeField]
	private float damageAdder = 5f;
	[SerializeField]
	private float hitForceAdder = 20f;
	[SerializeField]
	private List<TrailRenderer> trails;
	[SerializeField]
	private GameObject particleEffect;

	private bool isKillEffect;
	private bool isProjectile;

    private void Start()
    {
		foreach (TrailRenderer trail in trails)
		{
			trail.enabled = false;
		}
	}

    public override void SetAimColor(Gradient gradient)
	{
		laserAim.SetActive(true);
		GetComponentInChildren<LineRenderer>().colorGradient = gradient;
		laserAim.SetActive(false);
	}

	public override void ToggleAim(bool isActive)
	{
		laserAim.SetActive(isActive);
	}

	public override void StartAttack()
	{
		SpecialController.Animator.SetTrigger("isStartSpecialRush");
	}
	public override void Attack()
	{
		ActionPower = Charges;
		Charges = 0;
		SpecialController.Animator.SetTrigger("isSpecialRush");
	}

	public override void StartTurnEffect()
	{
		base.AddCharge();
	}
	public override void GiveRegularDamageEffect()
	{
		base.AddCharge();
	}
	public override void KillEffect()
	{
		isKillEffect = true;
	}

	public override void DoSpecialAction()
	{
		isKillEffect = false;
		AkSoundEngine.PostEvent("VickySlide", gameObject);
		
		if (ActionPower == 1)
		{
			HolderAgent.GetComponent<Rigidbody>().AddForce(HolderAgent.transform.forward * rushForce, ForceMode.VelocityChange);
		}
		else
		{
			HolderAgent.GetComponent<Rigidbody>().AddForce(HolderAgent.transform.forward * (rushForce + rushForceAdder), ForceMode.VelocityChange);
		}
		
		isProjectile = true;

		foreach(TrailRenderer trail in trails)
        {
			trail.enabled = true;
        }

		StartCoroutine(CountdownTime(0.5f));
	}

	private IEnumerator CountdownTime(float time)
	{
		yield return new WaitForSeconds(time);
		if (isProjectile)
		{
			EndSpecial();
		}
	}

	private void EndSpecial()
	{
		isProjectile = false;
		ActionPower = 0;
		foreach (TrailRenderer trail in trails)
		{
			trail.enabled = false;
		}
		//base.DoSpecialAction();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (isProjectile)
		{
			if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
			{
				Vector3 forceDirection = other.transform.position - transform.position;
				forceDirection.y = 0;
				forceDirection.Normalize();

				Instantiate(particleEffect, transform.position, transform.rotation);

				switch (ActionPower)
				{
					case 1:
						Effects.WeaponDamage(other.gameObject, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
						Effects.ApplyForce(other.gameObject, forceDirection * HitForce);
						break;
					case 2:
						Effects.WeaponDamage(other.gameObject, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
						Effects.ApplyForce(other.gameObject, forceDirection * (HitForce + hitForceAdder));
						break;
					case 3:
						Effects.WeaponDamage(other.gameObject, (Damage + (2 * damageAdder)) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), HolderAgent);
						Effects.ApplyForce(other.gameObject, forceDirection * (HitForce + hitForceAdder));
						Effects.ApplyWeaponEffects(other.gameObject, effects);
						if (isKillEffect)
						{
							Charges = MaxCharges;
						}
						break;
				}
				EndSpecial();
			}
			else if (other.gameObject.layer == LayerMask.NameToLayer("Destructible"))
			{
				switch (ActionPower)
				{
					case 1:
						Effects.Damage(other.gameObject, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
						break;
					case 2:
						Effects.Damage(other.gameObject, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
						break;
					case 3:
						Effects.Damage(other.gameObject, (Damage + (2 * damageAdder)) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
						break;
				}
				EndSpecial();
			}
		}
	}
}
