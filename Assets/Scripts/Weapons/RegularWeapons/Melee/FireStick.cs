using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStick : SwingWeapon
{
    [Range(0,3)]
    [SerializeField] private int fireStacks = 1;

	[Range(0, 5)]
	[SerializeField] private int duration = 2;

    public override void DoAction(FieldOfView fov)
    {
		if (fov.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in fov.VisibleTargets)
			{
				if(target.tag != "CoverObject")
				{
					Effects.RegularWeaponDamage(target, Damage, HolderAgent);
					Effects.ApplyForce(target, (target.transform.position - fov.transform.position).normalized * HitForce);
					Effects.ApplyStatusEffect(target, StatusEffectType.StaminaDrain, 3, fireStacks);
				}
				else
				{
					Effects.Damage(target, Damage);
				}
			}
		}
	}
}
