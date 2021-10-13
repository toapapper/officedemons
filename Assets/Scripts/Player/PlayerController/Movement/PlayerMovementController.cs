using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
	private Rigidbody rb;
	private NavMeshAgent navmeshAgent;
    //Character movers
    //private CharacterController character;
    //private WeaponHand weaponHand;
	//private SpecialHand specialHand;

	//Movement variables
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 moveAmount = Vector3.zero;
	private Vector3 smoothMoveVelocity;
	private Quaternion rotationDirection;

	//Movement speeds
	[SerializeField]
	private float moveSpeed = 10f;
	[SerializeField]
	private float rotationSpeed = 500f;

	public float getMoveSpeed{ get{ return moveSpeed; }}

	////Throwing variables
	//[SerializeField]
	//private float throwForceMultiplier = 25f;
	//[SerializeField]
	//private float maxThrowForce = 30f;
	//private float addedThrowForce;

	////Bombard variables
	//[SerializeField]
	//private float bombardForceMultiplier = 5f;
	//[SerializeField]
	//private float maxBombardForce = 10f;
	//private float addedBombardForce;


	////Healing variables
	//[SerializeField]
	//private int maxHealthMark = 100;
	//private int lowestHealth;


	public Vector3 MoveDirection
	{
		get { return moveDirection; }
		set { moveDirection = value; }
	}
	public Vector3 MoveAmount
	{
		get { return moveAmount; }
		set { moveAmount = value; }
	}


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		navmeshAgent = GetComponent<NavMeshAgent>();
		//character = GetComponent<CharacterController>();
		//weaponHand = GetComponent<WeaponHand>();
		//specialHand = GetComponent<SpecialHand>();
	}

	//public void SetMoveDirection(Vector2 moveInput)
	//{
	//	moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
	//}

	//TODO Added force to player input
	//public bool PerformThrow()
	//{
	//	if (weaponHand.Throw(addedThrowForce))
	//	{
	//		addedThrowForce = 0;
	//		return true;
	//	}
	//	return false;
	//}
	//public void AddThrowForce()
	//{
	//	if (addedThrowForce < maxThrowForce)
	//	{
	//		addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
	//	}
	//}
	//public void CancelThrow()
	//{
	//	weaponHand.CancelAction();
	//	addedThrowForce = 0;
	//}

	//public bool PerformBombard()
	//{
	//	if (weaponHand.PerformBombard(addedBombardForce))
	//	{
	//		addedBombardForce = 0;
	//		return true;
	//	}
	//	return false;
	//}
	//public void AddBombardForce()
	//{
	//	if(addedBombardForce < maxBombardForce)
	//	{
	//		addedBombardForce += bombardForceMultiplier * Time.fixedDeltaTime;
	//		weaponHand.SetBombardForce(addedBombardForce);
	//	}
	//}
	//public void CancelBombard()
	//{
	//	weaponHand.CancelAction();
	//	addedBombardForce = 0;
	//	weaponHand.SetBombardForce(addedBombardForce);
	//}

	//Calculate movement
	public Quaternion CalculateRotation()
	{
		if(moveDirection != Vector3.zero)
		{
			rotationDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
		}
		return rotationDirection;
	}
	public Vector3 CalculateMovement()
	{
		Debug.Log(navmeshAgent.velocity.magnitude);
		Vector3 targetMoveAmount = moveDirection * moveSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
		return moveAmount;
	}
	//Perform movement
	public void PerformRotation()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);
	}
	public void PerformMovement()
	{
		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position + moveAmount * Time.fixedDeltaTime);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		pos = Camera.main.ViewportToWorldPoint(pos);
		pos.y = Mathf.Clamp(pos.y, 0, 1.05f);

		navmeshAgent.Move(pos - transform.position);
		//rb.MovePosition(pos);

		//rb.velocity = (pos - transform.position) / Time.fixedDeltaTime; // with clamping to screen

		//rb.velocity = moveAmount; //no clamping to screen

		//character.Move(moveAmount * Time.fixedDeltaTime);
		//rb.AddForce(moveAmount * Time.fixedDeltaTime, ForceMode.VelocityChange);
		//rb.MovePosition(transform.position + moveAmount * Time.fixedDeltaTime);
	}
}
