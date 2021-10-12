using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombardWeapon : AbstractWeapon
{
	[SerializeField]
	private GameObject granade;
	private int granadeThrowForce;

	public int GranadeThrowForce
	{
		get { return granadeThrowForce; }
		set { granadeThrowForce = value; }
	}

	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartBombard");
	}

	public override void Attack(Animator animator)
	{
		animator.SetTrigger("isBombard");
	}

	public override void DoAction(FieldOfView fov)
	{
		Vector3 forward = transform.forward;
		forward.y = 0;
		forward.Normalize();
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 direction = (Quaternion.AngleAxis(-GetComponentInParent<WeaponHand>().ThrowAim.initialAngle, right) * forward).normalized;
		float throwForce = GetComponentInParent<WeaponHand>().ThrowAim.initialVelocity;

		granade.GetComponent<GranadeObject>().CreateGranade(transform.position, direction, throwForce, HitForce, Damage);
	}
}
