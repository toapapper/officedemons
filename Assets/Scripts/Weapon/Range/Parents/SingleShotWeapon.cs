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
public class SingleShotWeapon : RangedWeapon
{
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isRangedSingleShot");
	}
}
