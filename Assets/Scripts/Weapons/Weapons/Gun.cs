using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public class Gun : SingleShotWeapon
{
	[SerializeField]
	private GameObject gunHandle;
	[SerializeField]
	private int gunDamage = 10;
	[SerializeField]
	private int gunThrowDamage = 2;
	[SerializeField]
	private float gunViewDistance = 20f;
	[SerializeField]
	private float gunViewAngle = 10f;

	private void Start()
	{
		Handle = gunHandle;
		Damage = gunDamage;
		ThrowDamage = gunThrowDamage;
		ViewDistance = gunViewDistance;
		ViewAngle = gunViewAngle;
	}
}
