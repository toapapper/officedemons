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


	public override void SetAim(FieldOfView fov, GameObject fovVisualization, GameObject throwAim, Gradient gradient)
	{
		fovVisualization.GetComponent<Renderer>().material.color = gradient.colorKeys[0].color;
		fov.ViewAngle = viewAngle;
		fov.ViewRadius = viewDistance;
	}
	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		FOVView.SetActive(isActive);
	}

	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartSpecialBreath");
	}
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isSpecialBreath");
		
	}


	//public void StartFire()
	//{
	//	//Activate fire from mouth
	//}
	public override void DoSpecialAction(FieldOfView fov)
	{
		if (fov.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in fov.VisibleTargets)
			{
				Effects.Damage(target, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost));
				Effects.ApplyForce(target, (target.transform.position - fov.transform.position).normalized * HitForce);
				Effects.ApplyWeaponEffects(target, effects);
			}
		}
	}
	//public void EndFire()
	//{
	//	//Deactivate fire from mouth
	//}
}
