using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
	private GameObject handle;

	private bool isHeld;
    private bool isProjectile;

    private float damage = 10f;
    private float throwDamage = 15f;

	public bool IsHeld
	{
		get { return isHeld; }
		set { isHeld = value; }
	}
	public bool IsProjectile
	{
		get { return isProjectile; }
		set { isProjectile = value; }
	}
	protected float Damage
	{
		get { return damage; }
		set { damage = value; }
	}
	protected float ThrowDamage
	{
		get { return throwDamage; }
		set { throwDamage = value; }
	}
	protected GameObject Handle
	{
		get { return handle; }
		set { handle = value; }
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
	public void Drop(Vector3 direction)
	{
		handle.transform.parent = null;
		handle.GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().isKinematic = false;
		//handle.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
		isHeld = false;
	}

	public abstract void Hit(Animator animator);

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
		//else if (collision.gameObject.tag == "Player")
		//{
		//	Physics.IgnoreCollision(collision.gameObject.GetComponents<CharacterController>(), GetComponent<Collider>());
		//}
	}
}
