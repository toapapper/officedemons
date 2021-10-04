using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public abstract class RangedWeapon : AbstractWeapon
{
	private GameObject weaponMuzzle;
	private GameObject laserAim;
	[SerializeField]
	private GameObject bullet;
	private int bulletHitForce;
	private int bulletFireForce;

	protected GameObject WeaponMuzzle
	{
		get { return weaponMuzzle; }
		set { weaponMuzzle = value; }
	}
	protected GameObject LaserAim
	{
		get { return laserAim; }
		set { laserAim = value; }
	}
	public int BulletFireForce
	{
		get { return bulletFireForce; }
		set { bulletFireForce = value; }
	}
	public int BulletHitForce
	{
		get { return bulletHitForce; }
		set { bulletHitForce = value; }
	}

	public override void Shoot()
	{
		Vector3 bulletDirection = transform.forward;
		bulletDirection.y = 0;
		bulletDirection.Normalize();

		//GameObject newBullet = bullet;
		//Instantiate(newBullet, WeaponMuzzle.transform.position + forward, Quaternion.LookRotation(forward));
		//newBullet.GetComponent<Rigidbody>().AddForce(forward * bulletForce, ForceMode.VelocityChange);
		Debug.Log(Damage);
		bullet.GetComponent<Bullet>().CreateBullet(WeaponMuzzle.transform.position, bulletDirection, BulletFireForce, BulletHitForce, Damage);



		//Bullet bullet = new Bullet(WeaponMuzzle.transform.position, forward);

		//bullet = Instantiate(bullet, WeaponMuzzle.transform.position, Quaternion.LookRotation(forward));

		//bullet.SendMessage("SetDamage", Damage);


		//bullet = new Bullet(WeaponMuzzle.transform.position, forward, Damage);
		//newBullet.GetComponent<Rigidbody>().AddForce(Quaternion.LookRotation(forward) * bulletForce, ForceMode.VelocityChange);

		//RaycastHit hit;
		//if (Physics.Raycast(WeaponMuzzle.transform.position, forward, out hit, maxBulletDistance, ~ignoreLayer))
		//{
		//		if (hit.collider != null)
		//		{
		//			Actions targetActions = hit.transform.GetComponent<Actions>();
		//			if (targetActions != null)
		//			{
		//				targetActions.TakeBulletDamage(Damage, WeaponMuzzle.transform.position);
		//			}
		//		}
		//}
		//else
		//{
		//	//SpawnBullet() maxDistance
		//}


	}
	public override void ToggleLaserAim(bool isActive, Gradient laserSightMaterial)
	{
		LaserAim.SetActive(isActive);
		GetComponentInChildren<LineRenderer>().colorGradient = laserSightMaterial;
	}
	public override void StartAttack(Animator animator)
	{
		animator.SetTrigger("isStartRangedSingleShot");
	}
	public override abstract void Attack(Animator animator);
}
