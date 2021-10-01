using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coded by: Johan Melkersson
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
    //Character movers
    private CharacterController character;
    private WeaponHand weaponHand;
	//private SpecialHand specialHand;

	//World transform variables
	private Vector3 forward;
	private Vector3 right;

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

	//Helper variables
	private List<GameObject> nearbyObjects = new List<GameObject>();
	private List<GameObject> nearbyPlayers = new List<GameObject>();
	public bool isWeaponEquipped;


	private void Awake()
	{
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward.Normalize();
		right = new Vector3(forward.z, 0, -forward.x);

		character = GetComponent<CharacterController>();
        weaponHand = GetComponent<WeaponHand>();
        //specialHand = GetComponent<SpecialHand>();
    }

	//Pickup
	public void PerformPickup()
	{
		if (weaponHand != null && nearbyObjects.Count > 0)
		{
			foreach (GameObject nearbyObject in nearbyObjects)
			{
				if (!nearbyObject.GetComponentInChildren<AbstractWeapon>().IsHeld)
				{
					weaponHand.Equip(nearbyObject);
					isWeaponEquipped = true;
					break;
				}
			}
		}
	}

	//Throw
	public bool StartThrow()
	{
		if (weaponHand != null)
		{
			weaponHand.StartThrow();
			return true;
		}
		return false;
	}
	public bool PerformThrow()
	{
		if (weaponHand != null)
		{
			weaponHand.Throw(addedThrowForce);
			isWeaponEquipped = false;
			addedThrowForce = 0;
			return true;
		}
		return false;
	}
	public void CancelThrow()
	{
		//TODO
		//weaponHand.CancelThrow();
	}
	public void AddThrowForce()
	{
		if (addedThrowForce <= maxThrowForce)
		{
			addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
		}
	}

	//Attack
	public void StartAttack()
	{
		if (weaponHand != null)
		{
			weaponHand.StartAttack();
		}
	}
	public void PerformAttack()
	{
		if (weaponHand != null)
		{
			weaponHand.Attack();
		}
	}
	public void CancelAttack()
	{
		if (weaponHand != null)
		{
			weaponHand.CancelAction();
		}
	}

	//Special attack
	public void StartSpecial()
	{
		////TODO
		//if ((specialHand != null)
		//{
		//	specialHand.StartSpecial();
		//}
	}
	public void PerformSpecial()
	{
		////TODO
		//if ((specialHand != null)
		//{
		//	specialHand.PerformSpecial();
		//}
	}
	public void CancelSpecial()
	{
		////TODO
		//if ((specialWeaponHand != null)
		//{
		//	specialHand.CancelSpecial();
		//}
	}

	//Revive
	public bool StartRevive()
	{
		if (nearbyPlayers.Count > 0)
		{
			foreach (GameObject nearbyPlayer in nearbyPlayers)
			{
				if (nearbyPlayer.GetComponentInChildren<Attributes>().Health <= 0)
				{
					return true;
				}
			}
		}
		return false;
	}
	public void PerformRevive()
	{
		if (nearbyPlayers.Count > 0)
		{
			foreach (GameObject nearbyPlayer in nearbyPlayers)
			{
				if (nearbyPlayer.GetComponentInChildren<Attributes>().Health <= 0)
				{
					Debug.Log("Revive player " + nearbyPlayer.name);
					GetComponent<Actions>().Revive(nearbyPlayer);
					return;
				}
			}
		}
	}
	public void CancelRevive()
	{

	}

	//Toggle Aim
	public void ToggleWeaponAimView(bool isActive)
	{
		if(weaponHand != null)
		{
			weaponHand.ToggleAimView(isActive);
		}
	}
	public void ToggleSpecialAimView(bool isActive)
	{
		//TODO
		//if (specialHand != null)
		//{
		//	specialHand.ToggleAimView(isActive);
		//}
	}
	public void ToggeThrowAimView(bool isActive)
	{
		if (weaponHand != null)
		{
			//TODO
			//weaponHand.ToggeThrowAimView(isActive);
		}
	}

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

	//Set Movement
	public void SetMoveDirection(Vector2 moveInput)
	{
		moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
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
	}
	//Perform movement
	public void PerformRotation()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);
	}
	public void PerformMovement()
	{
		character.Move(moveAmount * Time.fixedDeltaTime);
	}
	public void PerformFall()
	{
		character.Move(Vector3.down * Time.fixedDeltaTime);
	}



	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "WeaponObject")
		{
			nearbyObjects.Add(other.gameObject);
		}
		else if(other.gameObject.tag == "Player")
		{
			nearbyPlayers.Add(other.gameObject);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "WeaponObject")
		{
			nearbyObjects.Remove(other.gameObject);
		}
		else if(other.gameObject.tag == "Player")
		{
			nearbyPlayers.Remove(other.gameObject);
		}
	}
}
