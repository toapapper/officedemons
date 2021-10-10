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
	private GameObject uziMuzzle;
	[SerializeField]
	private GameObject uziLaserAim;
	[SerializeField]
	private int uziDamage = 4;
	[SerializeField]
	private int uziBulletFireForce = 20;
	[SerializeField]
	private int uziBulletHitForce = 10;
	[SerializeField]
	private int uziThrowDamage = 2;
	[SerializeField]
	private float uziViewDistance = 20f;
	[SerializeField]
	private float uziViewAngle = 10f;

	private void Start()
	{
		Handle = uziHandle;
		WeaponMuzzle = uziMuzzle;
		LaserAim = uziLaserAim;
		Damage = uziDamage;
		BulletFireForce = uziBulletFireForce;
		HitForce = uziBulletHitForce;
		ThrowDamage = uziThrowDamage;
		ViewDistance = uziViewDistance;
		ViewAngle = uziViewAngle;
	}
}
