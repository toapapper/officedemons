using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public class Uzi : BurstShotWeapon
{
	[SerializeField]
	private GameObject uziHandle;
	[SerializeField]
	private int uziDamage = 4;
	[SerializeField]
	private int uziThrowDamage = 2;
	[SerializeField]
	private float uziViewDistance = 20f;
	[SerializeField]
	private float uziViewAngle = 10f;

	private void Start()
	{
		Handle = uziHandle;
		Damage = uziDamage;
		ThrowDamage = uziThrowDamage;
		ViewDistance = uziViewDistance;
		ViewAngle = uziViewAngle;
	}
}
