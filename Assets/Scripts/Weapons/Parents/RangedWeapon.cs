using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class RangedWeapon : AbstractWeapon
{
    public override abstract void Attack(Animator animator);
}
