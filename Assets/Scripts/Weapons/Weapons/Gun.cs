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
	private GameObject gunMuzzle;
	[SerializeField]
	private GameObject laserAim;
	[SerializeField]
	private int gunDamage = 10;
	[SerializeField]
	private int gunBulletFireForce = 5;
	[SerializeField]
	private int gunBulletHitForce = 100;
	[SerializeField]
	private int gunThrowDamage = 2;
	[SerializeField]
	private float gunViewDistance = 20f;
	[SerializeField]
	private float gunViewAngle = 10f;

	private void Start()
	{
		Handle = gunHandle;
		WeaponMuzzle = gunMuzzle;
		LaserAim = laserAim;
		Damage = gunDamage;
		BulletFireForce = gunBulletFireForce;
		BulletHitForce = gunBulletHitForce;
		ThrowDamage = gunThrowDamage;
		ViewDistance = gunViewDistance;
		ViewAngle = gunViewAngle;
	}
}
