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
		specialController.Animator.SetTrigger("isStartSpecialRush");
	}
	public override void Attack()
	{
		specialController.Animator.SetTrigger("isSpecialRush");
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

		if (Charges == 0)
		{
			holderAgent.GetComponent<Rigidbody>().AddForce(holderAgent.transform.forward * rushForce, ForceMode.VelocityChange);
		}
		else
		{
			holderAgent.GetComponent<Rigidbody>().AddForce(holderAgent.transform.forward * (rushForce + rushForceAdder), ForceMode.VelocityChange);
		}
		
		isProjectile = true;
		GameManager.Instance.StillCheckList.Add(holderAgent);

		foreach(TrailRenderer trail in trails)
        {
			trail.enabled = true;
        }

		StartCoroutine(CountdownTime(2));
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
		GameManager.Instance.StillCheckList.Remove(holderAgent);

		foreach (TrailRenderer trail in trails)
		{
			trail.enabled = false;
		}
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

				if (Charges == 0)
				{
					Effects.Damage(other.gameObject, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
					Effects.ApplyForce(other.gameObject, forceDirection * HitForce);
					Charges = 0;
				}
				else if(Charges == 1)
				{
					Effects.Damage(other.gameObject, (Damage + damageAdder) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
					Effects.ApplyForce(other.gameObject, forceDirection * (HitForce + hitForceAdder));
					Charges = 0;
				}
				else
				{
					Effects.Damage(other.gameObject, (Damage +(2 * damageAdder)) * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
					Effects.ApplyForce(other.gameObject, forceDirection * (HitForce + hitForceAdder));
					Effects.ApplyWeaponEffects(other.gameObject, effects);
					if (!isKillEffect)
					{
						Charges = 0;
					}
				}
			}
			else
			{
				Charges = 0;
			}
			EndSpecial();
		}
	}
}
