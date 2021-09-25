using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeapon : MeleeWeapon
{
	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isMeleeSwing");
		Debug.Log("MeleeSwing" + Damage);
	}
}
