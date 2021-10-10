using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombardWeapon : AbstractWeapon
{
	//private GameObject throwAim;
	[SerializeField]
	private GameObject granade;
	private int granadeThrowForce;

	//protected GameObject ThrowAim
	//{
	//	get { return throwAim; }
	//	set { throwAim = value; }
	//}
	public int GranadeThrowForce
	{
		get { return granadeThrowForce; }
		set { granadeThrowForce = value; }
	}
	public override void ReleaseProjectile()
	{
		Vector3 granadeDirection = transform.forward;
		granadeDirection.y = 0;
		granadeDirection.Normalize();

		Vector3 forward = transform.forward;
		forward.y = 0;
		forward.Normalize();
		//Vector3 direction = (Quaternion.AngleAxis(-throwAim.GetComponent<ThrowAim>().initialAngle, transform.right) * forward).normalized;
		//granade.GetComponent<GranadeObject>().CreateGranade(transform.position, direction,
		//	throwAim.GetComponent<ThrowAim>().initialVelocity, HitForce, Damage);

		Vector3 direction = (Quaternion.AngleAxis(-GetComponentInParent<WeaponHand>().ThrowAim.initialAngle, transform.right) * forward).normalized;
		granade.GetComponent<GranadeObject>().CreateGranade(transform.position, direction,
			GetComponentInParent<WeaponHand>().ThrowAim.initialVelocity, HitForce, Damage);
	}
	//public override void ToggleAim(bool isActive, Gradient laserSightMaterial)
	//{
	//	throwAim.SetActive(isActive);
	//	if (isActive)
	//	{
	//		GetComponentInChildren<LineRenderer>().colorGradient = laserSightMaterial;
	//	}
	//}
	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartBombard");
	}

	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isBombard");
	}
}
