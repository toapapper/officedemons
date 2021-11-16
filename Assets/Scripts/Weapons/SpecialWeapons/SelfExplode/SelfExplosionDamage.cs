using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfExplosionDamage : AbstractSpecial
{
	[SerializeField] private List<WeaponEffects> ultiEffects;

	[SerializeField]
	private float viewDistance = 1f;
	[SerializeField]
	private float viewAngle = 360f;
	[SerializeField]
	private float distanceMultiplier = 1f;


	public override void SetFOVSize()
	{
		specialController.FOV.ViewAngle = viewAngle;
		specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
	}

	public override void ToggleAim(bool isActive)
	{
		specialController.FOVVisualization.SetActive(isActive);
	}

	public override void StartAttack()
	{
		specialController.Animator.SetTrigger("isStartSpecialPaperShredder");
	}
	public override void Attack()
	{
		specialController.Animator.SetTrigger("isSpecialPaperShredder");
	}

	public override void TakeDamageEffect()
	{
		AddCharge();
	}
	public override void AddCharge()
	{
		base.AddCharge();

		specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
		if (Charges == MaxCharges)
		{
			Attack();
		}
	}


	//public override void StartSpecialAction()
	//{

	//}
	public override void DoSpecialAction()
	{
		if (specialController.FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in specialController.FOV.VisibleTargets)
			{
				Debug.Log("Charges     " + Charges + "MAX     " + MaxCharges);
				if(Charges < MaxCharges)
				{
					Debug.Log("LOW");
					Effects.ApplyWeaponEffects(target, effects);
				}
				else
				{
					Debug.Log("HIGH");
					Effects.Damage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), holderAgent);
					Effects.ApplyForce(target, (target.transform.position - specialController.FOV.transform.position).normalized * HitForce);
					Effects.ApplyWeaponEffects(target, ultiEffects);
				}				
			}
			Charges = 0;
			specialController.FOV.ViewRadius = viewDistance + (distanceMultiplier * Charges);
		}
	}
	//public override void EndSpecialAction()
	//{

	//}
}
