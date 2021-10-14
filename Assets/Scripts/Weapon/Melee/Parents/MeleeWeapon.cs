using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : AbstractWeapon
{
	//[SerializeField]
	//private GameObject meleeHandle;
	//[SerializeField]
	//private int meleeDamage = 10;
	//[SerializeField]
	//private int meleeHitForce = 15;
	//[SerializeField]
	//private int meleeThrowDamage = 15;
	//[SerializeField]
	//private float meleeViewDistance = 3.5f;
	//[SerializeField]
	//private float meleeViewAngle = 20f;

	//private void Start()
	//{
	//	Handle = meleeHandle;
	//	Damage = meleeDamage;
	//	HitForce = meleeHitForce;
	//	ThrowDamage = meleeThrowDamage;
	//	ViewDistance = meleeViewDistance;
	//	ViewAngle = meleeViewAngle;
	//}

	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		FOVView.SetActive(isActive);
	}
	public override abstract void Attack(Animator animator);
	public override void DoAction(FieldOfView fov)
	{
		if (fov.visibleTargets.Count > 0)
		{
			foreach (GameObject target in fov.visibleTargets)
			{
				Effects.Damage(target, Damage);
				Effects.ApplyForce(target, (target.transform.position - fov.transform.position).normalized * HitForce);
			}
		}
	}
}
