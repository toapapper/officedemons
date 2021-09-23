using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustWeapon : MeleeWeapon
{
	public override void Hit(Animator animator)
	{
		animator.SetTrigger("isMeleeThrust");
		Debug.Log("MeleeTHrust" + Damage);
	}
}
