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

// Last Edited: 15-12-21
public class RushChair : AbstractSpecial
{
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private float rushForce = 100f;
	[SerializeField]
	private float damageAdder = 5f;
	[SerializeField]
	private float hitForceAdder = 20f;
	[SerializeField]
	private List<TrailRenderer> trails;
	[SerializeField]
	private GameObject particleEffect;
	[SerializeField]
	private GameObject explosionParticleEffect;

	private bool refundable;
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
				ActionPower = 2.5f;
				break;
			case 3:
				ActionPower = 4;
				break;
            default:
				ActionPower = 0;
                break;
        }
		SpecialController.Animator.SetTrigger("isSpecialRush");
	}

	public override void StartTurnEffect()
	{
		//base.AddCharge();
	}
	public override void GiveRegularDamageEffect()
	{
		base.AddCharge();
	}
	public override void KillEffect()
	{
        if (refundable)
        {
			//gameObject.transform.parent.transform.GetComponentInChildren<StatusEffectHandler>().ApplyEffect(StatusEffectType.damage_boost, HolderAgent);
			isKillEffect = true;
        }
	}

	public override void DoSpecialAction()
	{
        if (Charges >= MaxCharges)
        {
			Charges = MaxCharges;
			refundable = true;
        }
		AkSoundEngine.PostEvent("VickySlide", gameObject);

		HolderAgent.GetComponent<Rigidbody>().AddForce(HolderAgent.transform.forward * rushForce * ActionPower, ForceMode.VelocityChange);

		
		isProjectile = true;

		foreach(TrailRenderer trail in trails)
        {
			trail.enabled = true;
        }

		StartCoroutine(CountdownTime(0.5f));
		ActionPower = 0;
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
		//base.DoSpecialAction();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (isProjectile)
		{
			StartCoroutine(Invulnerable(0.5f));
			if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
			{
				Vector3 forceDirection = other.transform.position - transform.position;
				forceDirection.y = 0;
				forceDirection.Normalize();

				Instantiate(particleEffect, transform.position, transform.rotation);
				Instantiate(explosionParticleEffect, other.gameObject.transform.position, transform.rotation);
				switch (Charges)
				{
					case 1:
						Effects.WeaponDamage(other.gameObject, Damage * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), HolderAgent);
						Effects.ApplyForce(other.gameObject, forceDirection * HitForce * ActionPower);
						break;
					case 2:
						Effects.WeaponDamage(other.gameObject, (Damage + (damageAdder * ActionPower)) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), HolderAgent);
						Effects.ApplyForce(other.gameObject, forceDirection * HitForce * ActionPower);
						break;
					case 3:
						Effects.WeaponDamage(other.gameObject, (Damage + (damageAdder * ActionPower)) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), HolderAgent);
						Effects.ApplyForce(other.gameObject, forceDirection * HitForce * ActionPower);
						break;
				}
				EndSpecial();
			}
			else if (other.gameObject.layer == LayerMask.NameToLayer("Destructible"))
			{
				Instantiate(explosionParticleEffect, other.gameObject.transform.position, transform.rotation);
				switch (Charges)
				{
					case 1:
						Effects.Damage(other.gameObject, Damage * ActionPower * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost));
						break;
					case 2:
						Effects.Damage(other.gameObject, (Damage + (damageAdder * ActionPower)) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost));
						break;
					case 3:
						Effects.Damage(other.gameObject, (Damage + (damageAdder * ActionPower)) * (1 + GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost));
						break;
				}
				EndSpecial();
			}
		}
	}

	private IEnumerator Invulnerable(float time)
	{
		HolderAgent.GetComponent<Attributes>().invulnerable = true;
		yield return new WaitForSeconds(time);
		HolderAgent.GetComponent<Attributes>().invulnerable = false;
		yield return null;
	}

}
