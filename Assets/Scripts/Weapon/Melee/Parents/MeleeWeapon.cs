using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Summary of what the component does 
/// 
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
///  
/// </para>
///  
/// </summary>

// Last Edited: 14/10-21
public abstract class MeleeWeapon : AbstractWeapon
{
	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		FOVView.SetActive(isActive);
	}
	public override abstract void Attack(Animator animator);
	public override void DoAction(FieldOfView fov)
	{
		if (fov.visibleTargets.Count > 0)
		{
			foreach (GameObject target in fov.visibleTargets)
			{
				Effects.Damage(target, Damage);
				Effects.ApplyForce(target, (target.transform.position - fov.transform.position).normalized * HitForce);
			}
		}
	}
}
