using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeSpecial : AbstractSpecial
{
	[SerializeField]
	private GameObject grenade;
	//[SerializeField]
	//private float grenadeThrowForce = 10;

	//public float GrenadeThrowForce
	//{
	//	get { return grenadeThrowForce; }
	//	set { grenadeThrowForce = value; }
	//}

	public override void ToggleAim(bool isActive)
	{
		if (!isActive)
		{
			specialController.ThrowAim.GetComponent<LineRenderer>().positionCount = 0;
		}
		specialController.ThrowAim.gameObject.SetActive(isActive);
	}

	public override void StartAttack()
	{
		specialController.Animator.SetTrigger("isStartSpecialBombard");
	}
	public override void Attack()
	{
		specialController.Animator.SetTrigger("isSpecialBombard");
	}

	public override void DoSpecialAction()
	{
		Vector3 forward = holderAgent.transform.forward;
		forward.y = 0;
		forward.Normalize();
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 direction = (Quaternion.AngleAxis(-specialController.ThrowAim.initialAngle, right) * forward).normalized;
		float throwForce = specialController.ThrowAim.initialVelocity;

		grenade.GetComponent<GrenadeObject>().CreateGrenade(transform.position, direction, throwForce, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);
	}
}
