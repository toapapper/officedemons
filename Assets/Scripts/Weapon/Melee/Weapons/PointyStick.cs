using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointyStick : ThrustWeapon
{
	[SerializeField]
	private GameObject pointyStickHandle;
	[SerializeField]
	private int pointyStickDamage = 10;
	[SerializeField]
	private int pointyStickHitForce = 15;
	[SerializeField]
	private int pointyStickThrowDamage = 15;
	[SerializeField]
	private float pointyStickViewDistance = 3.5f;
	[SerializeField]
	private float pointyStickViewAngle = 20f;

	private void Start()
	{
		Handle = pointyStickHandle;
		Damage = pointyStickDamage;
		HitForce = pointyStickHitForce;
		ThrowDamage = pointyStickThrowDamage;
		ViewDistance = pointyStickViewDistance;
		ViewAngle = pointyStickViewAngle;
	}
}
