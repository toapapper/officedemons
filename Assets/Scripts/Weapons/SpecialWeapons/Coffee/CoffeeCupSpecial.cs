using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCupSpecial : AbstractSpecial
{
	[SerializeField]
	protected GameObject grenade;
	[SerializeField]
	protected float explodeRadius;
	private bool isHit;
	
	//[SerializeField]
	//private float grenadeThrowForce = 10;

	//public float GrenadeThrowForce
	//{
	//	get { return grenadeThrowForce; }
	//	set { grenadeThrowForce = value; }
	//}

	public override void ToggleAim(bool isActive)
	{
		if (!isActive)
		{
			specialController.ThrowAim.GetComponent<LineRenderer>().positionCount = 0;
		}
		specialController.ThrowAim.gameObject.SetActive(isActive);
	}

	public override void StartAttack()
	{
		specialController.Animator.SetTrigger("isStartSpecialBombard");
	}
	public override void Attack()
	{
		specialController.Animator.SetTrigger("isSpecialBombard");
	}

	public override void StartTurnEffect()
	{
		if (!isHit)
		{
			base.AddCharge();
		}
		else
		{
			isHit = false;
		}

		base.AddCharge();
	}
	public override void TakeDamageEffect()
	{
		isHit = true;
	}

	public override void DoSpecialAction() { }

	//public override void DoSpecialAction()
	//{
	//	Vector3 forward = holderAgent.transform.forward;
	//	forward.y = 0;
	//	forward.Normalize();
	//	Vector3 right = new Vector3(forward.z, 0, -forward.x);

	//	Vector3 direction = (Quaternion.AngleAxis(-specialController.ThrowAim.initialAngle, right) * forward).normalized;
	//	float throwForce = specialController.ThrowAim.initialVelocity;
	//	List<WeaponEffects> currentEffects = effects;
	//	foreach(WeaponEffects effect in ultiEffects)
	//	{
	//		currentEffects.Add(effect);
	//	}
	//	if(Charges == 0)
	//	{
	//		grenade.GetComponent<SpecialGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
	//			explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects, ultiEffects);
	//	}
	//	else if(Charges == 1)
	//	{
	//		grenade.GetComponent<SpecialGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
	//			explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects, ultiEffects);
	//	}
	//	else
	//	{
	//		grenade.GetComponent<SpecialGrenade>().CreateGrenade(holderAgent, transform.position, direction, throwForce,
	//			explodeRadius, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects, ultiEffects);
	//	}
	//}
}
