using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointyStick : ThrustWeapon
{
	[SerializeField]
	private GameObject poleHandle;
	[SerializeField]
	private int poleDamage = 10;
	[SerializeField]
	private int poleThrowDamage = 15;
	[SerializeField]
	private float pointyStickViewDistance = 3f;
	[SerializeField]
	private float pointyStickViewAngle = 10f;

	private void Start()
	{
		Handle = poleHandle;
		Damage = poleDamage;
		ThrowDamage = poleThrowDamage;
		ViewDistance = pointyStickViewDistance;
		ViewAngle = pointyStickViewAngle;
	}
}
