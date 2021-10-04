using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public abstract class AbstractWeapon : MonoBehaviour
{
	private GameObject handle;
	private GameObject weaponMuzzle;

	public LineRenderer bulletTrail;
	public int maxBulletDistance = 30;
	[SerializeField]
	public LayerMask ignoreLayer;

	private bool isHeld;
    private bool isProjectile;

    private int damage;
    private int throwDamage;
	private float viewDistance;
	private float viewAngle;

	protected GameObject Handle
	{
		get { return handle; }
		set { handle = value; }
	}
	protected GameObject WeaponMuzzle
	{
		get { return weaponMuzzle; }
		set { weaponMuzzle = value; }
	}
	protected GameObject LaserAim
	{
		get { return weaponMuzzle; }
		set { weaponMuzzle = value; }
	}
	protected int ThrowDamage
	{
		get { return throwDamage; }
		set { throwDamage = value; }
	}
	public int Damage
	{
		get { return damage; }
		set { damage = value; }
	}
	public float ViewAngle
	{
		get { return viewAngle; }
		set { viewAngle = value; }
	}
	public float ViewDistance
	{
		get { return viewDistance; }
		set { viewDistance = value; }
	}
	public bool IsHeld
	{
		get { return isHeld; }
		set { isHeld = value; }
	}
	//public bool IsProjectile
	//{
	//	get { return isProjectile; }
	//	set { isProjectile = value; }
	//}



	public void PickUpIn(GameObject hand)
	{
		isHeld = true;
		handle.GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().isKinematic = true;

		handle.transform.parent = hand.transform;
		handle.transform.position = hand.transform.position;
		handle.transform.rotation = hand.transform.rotation;
	}
	public void Drop(Vector3 direction)
	{
		handle.transform.parent = null;
		handle.GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().isKinematic = false;
		isHeld = false;
	}
	//public virtual void DisplayAim() { }
	//public virtual void DisplayFov(GameObject fovView) { }
	public virtual void StartAttack(Animator animator) { }
	public abstract void Attack(Animator animator);
	public virtual void ToggleLaserAim(bool isActive, Gradient laserSightMaterial) { }
	public virtual void Shoot() { }
	

	public void ReleaseThrow(float force)
	{
		Drop(transform.up);

		GetComponentInChildren<Rigidbody>().AddForce(transform.up * force, ForceMode.VelocityChange);
		isProjectile = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isProjectile)
		{
			Debug.Log(throwDamage + "throwDamage to " + collision.gameObject);
			isProjectile = false;
		}
	}
}
