using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustWeapon : MeleeWeapon
{
	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartMeleeThrustAttack");
	}
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isMeleeThrust");
		Debug.Log("MeleeTHrust" + Damage);
	}
}
