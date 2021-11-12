using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushChairSpecial : AbstractSpecial
{
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private float rushForce = 100f;
	[SerializeField]
	private float rushForceAdder = 50f;
	[SerializeField]
	private float rushForceMultiplier = 10f;

	public bool isProjectile;


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

	public override void KillEffect()
	{
		Charges = MaxCharges;
	}

	public override void DoSpecialAction()
	{
		holderAgent.GetComponent<Rigidbody>().AddForce(holderAgent.transform.forward * (rushForce + (rushForceMultiplier * Charges)), ForceMode.VelocityChange);
		isProjectile = true;
		GameManager.Instance.StillCheckList.Add(holderAgent);
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

				if (Charges == 0)
				{
					Effects.Damage(other.gameObject, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
					Effects.ApplyForce(other.gameObject, forceDirection * HitForce);
				}
				else if(Charges == 1)
				{
					Effects.Damage(other.gameObject, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
					Effects.ApplyForce(other.gameObject, forceDirection * HitForce);
				}
				else
				{
					Effects.Damage(other.gameObject, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
					Effects.ApplyForce(other.gameObject, forceDirection * HitForce);
					Effects.ApplyWeaponEffects(other.gameObject, effects);
				}
				

				
			}
			isProjectile = false;
			GameManager.Instance.StillCheckList.Remove(holderAgent);
		}
	}
}
