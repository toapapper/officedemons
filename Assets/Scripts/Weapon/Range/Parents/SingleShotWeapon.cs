using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public class SingleShotWeapon : RangedWeapon
{
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isRangedSingleShot");
	}
}
