using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
 //   public bool isHeld;
 //   public bool isProjectile;

	//[SerializeField]
	//private GameObject handle;

	//[SerializeField]
 //   private float damage = 10f;
	//[SerializeField]
	//private float throwDamage = 15f;

	//public void PickUpIn(GameObject hand)
	//{
	//	isHeld = true;
	//	handle.GetComponent<Rigidbody>().useGravity = false;
	//	GetComponent<Rigidbody>().useGravity = false;
	//	handle.GetComponent<Rigidbody>().isKinematic = true;


	//	handle.transform.parent = hand.transform;
	//	handle.transform.position = hand.transform.position;
	//	handle.transform.rotation = hand.transform.rotation;
	//}
	//public void Drop(Vector3 direction)
	//{
	//	handle.transform.parent = null;
	//	handle.GetComponent<Rigidbody>().useGravity = true;
	//	GetComponent<Rigidbody>().useGravity = true;
	//	handle.GetComponent<Rigidbody>().isKinematic = false;
	//	GetComponent<Rigidbody>().isKinematic = false;
	//	handle.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
	//	isHeld = false;
	//}

 //   public  void Hit()
 //   {
 //       Debug.Log("Hit" + damage);
 //   }

	//public void Throw(Vector3 direction, float force)
	//{
	//	Drop(direction);

	//	handle.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
	//	GetComponentInChildren<Rigidbody>().AddForce(direction * force, ForceMode.VelocityChange);
	//	isProjectile = true;
	//}
	//private void OnCollisionEnter(Collision collision)
	//{
	//	if (isProjectile)
	//	{
	//		Debug.Log(collision.gameObject + "Take " + throwDamage + " damage");
	//		isProjectile = false;
	//	}
	//}
}
