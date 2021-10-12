using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : AbstractWeapon
{
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
