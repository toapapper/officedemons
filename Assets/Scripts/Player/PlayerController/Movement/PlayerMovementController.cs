using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
	private Rigidbody rb;
    //Character movers
    //private CharacterController character;
    private WeaponHand weaponHand;
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


	//Throwing variables
	[SerializeField]
	private float throwForceMultiplier = 25f;
	[SerializeField]
	private float maxThrowForce = 30f;
	private float addedThrowForce;

	//Healing variables
	[SerializeField]
	private int maxHealthMark = 100;
	private int lowestHealth;


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
	//public bool IsStaminaDepleted
	//{
	//	get { return attributes.Stamina <= 0; }
	//}


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		//character = GetComponent<CharacterController>();
        weaponHand = GetComponent<WeaponHand>();
        //specialHand = GetComponent<SpecialHand>();
    }

	//public void SetMoveDirection(Vector2 moveInput)
	//{
	//	moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
	//}

	//TODO Added force to player input
	public bool PerformThrow()
	{
		if (weaponHand.Throw(addedThrowForce))
		{
			//isWeaponEquipped = false;
			addedThrowForce = 0;
			return true;
		}
		return false;
	}
	public void CancelThrow()
	{
		weaponHand.CancelAction();
		addedThrowForce = 0;
	}
	public void AddThrowForce()
	{
		if (addedThrowForce <= maxThrowForce)
		{
			addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
		}
	}

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
		Vector3 targetMoveAmount = moveDirection * moveSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
		return moveAmount;

		//return moveDirection;
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
		//rb.MovePosition(pos);

		rb.velocity = moveAmount;

		//character.Move(moveAmount * Time.fixedDeltaTime);
		//rb.AddForce(moveAmount * Time.fixedDeltaTime, ForceMode.VelocityChange);
		//rb.MovePosition(transform.position + moveAmount * Time.fixedDeltaTime);

	}
	public void PerformFall()
	{
		//character.Move(Vector3.down * Time.fixedDeltaTime);
	}




	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.gameObject.tag == "WeaponObject")
	//	{
	//		nearbyObjects.Add(other.gameObject);
	//	}
	//	else if (other.gameObject.tag == "Player")
	//	{
	//		nearbyPlayers.Add(other.gameObject);
	//	}
	//}
	//private void OnTriggerExit(Collider other)
	//{
	//	if (other.gameObject.tag == "WeaponObject")
	//	{
	//		nearbyObjects.Remove(other.gameObject);
	//	}
	//	else if (other.gameObject.tag == "Player")
	//	{
	//		nearbyPlayers.Remove(other.gameObject);
	//	}
	//}



	////Heal
	//public bool StartHeal()
	//{
	//	if (GetComponent<Attributes>().Health < GetComponent<Attributes>().StartHealth)
	//	{
	//		return true;
	//	}
	//	if (nearbyPlayers.Count > 0)
	//	{
	//		foreach (GameObject nearbyPlayer in nearbyPlayers)
	//		{
	//			if (nearbyPlayer.GetComponentInChildren<Attributes>().Health < GetComponent<Attributes>().StartHealth)
	//			{
	//				return true;
	//			}
	//		}
	//	}
	//	return false;
	//}
	//public void PerformHeal()
	//{
	//	GameObject playerToHeal = null;
	//	if (GetComponent<Attributes>().Health < GetComponent<Attributes>().StartHealth)
	//	{
	//		playerToHeal = gameObject;
	//		lowestHealth = GetComponent<Attributes>().Health;
	//	}
	//	else
	//	{
	//		lowestHealth = GetComponent<Attributes>().StartHealth;
	//	}
	//	if (nearbyPlayers.Count > 0)
	//	{
	//		foreach (GameObject nearbyPlayer in nearbyPlayers)
	//		{
	//			if (nearbyPlayer.GetComponentInChildren<Attributes>().Health < lowestHealth)
	//			{
	//				playerToHeal = nearbyPlayer;
	//				lowestHealth = nearbyPlayer.GetComponentInChildren<Attributes>().Health;
	//			}
	//		}
	//	}
	//	if (playerToHeal != null)
	//	{
	//		Debug.Log("Heal player " + playerToHeal.name);
	//	}
	//}
	//public void CancelHeal()
	//{

	//}

}
