using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all melee weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public abstract class MeleeWeapon : AbstractWeapon
{
	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		FOVView.SetActive(isActive);
	}

	public override void Attack(Animator animator)
    {
		base.Attack(animator);
		base.Attack(animator);
	}

	public override void DoAction(FieldOfView fov)
	{
		if (fov.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in fov.VisibleTargets)
			{
				Effects.Damage(target, Damage);
				Effects.ApplyForce(target, (target.transform.position - fov.transform.position).normalized * HitForce);
			}
		}
	}
}
