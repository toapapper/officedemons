using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all swing weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-28
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
		base.Attack(animator);
	}
}
