using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeChairSpecial : AbstractSpecial
{
	public bool isProjectile;
	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		//if (!isActive)
		//{
		//	throwAim.GetComponent<LineRenderer>().positionCount = 0;
		//}
		//throwAim.SetActive(isActive);
	}

	public override void StartAttack(Animator animator)
	{
		//animator.SetTrigger("isStartSpecialBombard");
	}
	public override void Attack(Animator animator)
	{
		//animator.SetTrigger("isSpecialBombard");
	}

	public override void DoSpecialAction(FieldOfView fov)
	{
		//Vector3 forward = transform.parent.parent.forward;
		//forward.y = 0;
		//forward.Normalize();
		//Vector3 right = new Vector3(forward.z, 0, -forward.x);

		//Vector3 direction = (Quaternion.AngleAxis(-GetComponentInParent<SpecialHand>().ThrowAim.initialAngle, right) * forward).normalized;
		//float throwForce = GetComponentInParent<SpecialHand>().ThrowAim.initialVelocity;

		//grenade.GetComponent<GrenadeObject>().CreateGrenade(transform.position, direction, throwForce, HitForce, Damage * (1 + GetComponentInParent<StatusEffectHandler>().DmgBoost), effects);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isProjectile)
		{
			if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
			{
				
			}
		}
	}
}
