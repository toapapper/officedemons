using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public abstract class RangedWeapon : AbstractWeapon
{
	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartRangedSingleShot");
	}
	public override abstract void Attack(Animator animator);
}
