using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Abstract class controlling all weapons
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15/10-21
public abstract class AbstractWeapon : MonoBehaviour
{
	[SerializeField]
	private GameObject handle;
	[SerializeField]
	private float damage;
	[SerializeField]
	private float hitForce;
	[SerializeField]
	private float throwDamage = 2;
	[SerializeField]
	private float viewDistance = 20f;
	[SerializeField]
	private float viewAngle = 10f;

	[SerializeField]
	private bool isHeld;
	private bool isProjectile;

	protected GameObject Handle
	{
		get { return handle; }
		set { handle = value; }
	}
	public float Damage
	{
		get { return damage; }
		set { damage = value; }
	}
	public float HitForce
	{
		get { return hitForce; }
		set { hitForce = value; }
	}
	protected float ThrowDamage
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

	public virtual void SetAimGradient(Gradient gradient) { }
	public virtual void ToggleAim(bool isActive, GameObject FOVView, GameObject throwAim) { }
	public virtual void StartAttack(Animator animator) { }
	public abstract void Attack(Animator animator);
	public virtual void DoAction(FieldOfView fov) { }

	private void OnCollisionEnter(Collision collision)
	{
		if (isProjectile)
		{
			if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
			{
				Effects.Damage(collision.gameObject, throwDamage);
				isProjectile = false;
			}
		}
	}
}
