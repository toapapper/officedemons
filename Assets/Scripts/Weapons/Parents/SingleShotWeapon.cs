using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : RangedWeapon
{
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isRangedSingleShot");
		Debug.Log("RangedSingleShot" + Damage);
	}
}
