using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Methods connected to all bombard weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public class BombardWeapon : AbstractWeapon
{
	[SerializeField]
	private GameObject granade;
	[SerializeField]
	private float granadeThrowForce = 10;

	public float GranadeThrowForce
	{
		get { return granadeThrowForce; }
		set { granadeThrowForce = value; }
	}

	public override void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim)
	{
		if (!isActive)
		{
			throwAim.GetComponent<LineRenderer>().positionCount = 0;
		}
		throwAim.SetActive(isActive);
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

		granade.GetComponent<GrenadeObject>().CreateGrenade(transform.position, direction, throwForce, HitForce, Damage);
	}
}
