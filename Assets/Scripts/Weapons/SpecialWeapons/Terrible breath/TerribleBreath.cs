using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerribleBreath : AbstractSpecial
{
	[SerializeField]
	private float viewDistance = 10f;
	[SerializeField]
	private float viewAngle = 100f;
	[SerializeField]
	GameObject mouth;

	public override void SetFOVSize()
	{
		specialController.FOV.ViewAngle = viewAngle;
		specialController.FOV.ViewRadius = viewDistance;
	}

	public override void ToggleAim(bool isActive)
	{
		specialController.FOVVisualization.SetActive(isActive);
	}

	public override void StartAttack()
	{
		specialController.Animator.SetTrigger("isStartSpecialBreath");
	}
	public override void Attack()
	{
		specialController.Animator.SetTrigger("isSpecialBreath");
	}


	//public override void StartSpecialAction()
	//{
	//	//Activate fire from mouth
	//}
	public override void DoSpecialAction()
	{
		if (specialController.FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in specialController.FOV.VisibleTargets)
			{
				Effects.Damage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
				Effects.ApplyForce(target, (target.transform.position - specialController.FOV.transform.position).normalized * HitForce);
				Effects.ApplyWeaponEffects(target, effects);
			}
		}
	}
	//public override void StartSpecialAction()
	//{
	//	//Deactivate fire from mouth
	//}
}
