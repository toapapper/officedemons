using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstShotWeapon : RangedWeapon
{
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isMeleeSwing");
		Debug.Log("RangedBurstShot" + Damage);
	}
}
