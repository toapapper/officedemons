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
public class SwingWeapon : MeleeWeapon
{
	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartMeleeSwingAttack");
	}
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isMeleeSwing");
		Debug.Log("MeleeSwing " + Damage);
	}
}
