using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public abstract class AbstractWeapon : MonoBehaviour
{
	private GameObject handle;

	private int damage;
	private int hitForce;
	private int throwDamage;
	private float viewDistance;
	private float viewAngle;

	[SerializeField]
	private bool isHeld;
	private bool isProjectile;

	protected GameObject Handle
	{
		get { return handle; }
		set { handle = value; }
	}
	public int Damage
	{
		get { return damage; }
		set { damage = value; }
	}
	public int HitForce
	{
		get { return hitForce; }
		set { hitForce = value; }
	}
	protected int ThrowDamage
	{
		get { return throwDamage; }
		set { throwDamage = value; }
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

	public void PickUpIn(GameObject hand)
	{
		isHeld = true;
		handle.GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().isKinematic = true;

		handle.transform.parent = hand.transform;
		handle.transform.position = hand.transform.position;
		handle.transform.rotation = hand.transform.rotation;
	}
	public void ReleaseThrow(float force)
	{
		Drop();
		GetComponentInChildren<Rigidbody>().AddForce(transform.up * force, ForceMode.VelocityChange);
		isProjectile = true;
	}
	public void Drop()
	{
		handle.transform.parent = null;
		handle.GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().isKinematic = false;
		isHeld = false;
	}

	public virtual void StartAttack(Animator animator) { }
	public abstract void Attack(Animator animator);
	public virtual void ToggleAim(bool isActive, Gradient laserSightMaterial) { }
	public virtual void ReleaseProjectile() { }




	private void OnCollisionEnter(Collision collision)
	{
		if (isProjectile)
		{
			if (!collision.collider.isTrigger)
			{
				Debug.Log(throwDamage + "throwDamage to " + collision.gameObject);
				isProjectile = false;
			}
		}
	}
}
