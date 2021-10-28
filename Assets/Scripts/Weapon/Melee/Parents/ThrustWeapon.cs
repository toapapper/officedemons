using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all thrust weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public class ThrustWeapon : MeleeWeapon
{
	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartMeleeThrustAttack");
	}
	public override void Attack(Animator animator)
	{
		base.Attack(animator);
		animator.SetTrigger("isMeleeThrust");
		Debug.Log("MeleeTHrust" + Damage);
	}
}
