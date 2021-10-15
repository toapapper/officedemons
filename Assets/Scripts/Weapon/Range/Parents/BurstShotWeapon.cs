using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all burst weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public class BurstShotWeapon : RangedWeapon
{
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isRangedBurstShot");
	}
}
