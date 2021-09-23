using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : AbstractWeapon
{
	//private Bounds hitbox;



	public override abstract void Hit(Animator animator);
}
