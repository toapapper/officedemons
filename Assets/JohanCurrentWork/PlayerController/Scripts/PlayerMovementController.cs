using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private int lowHeathMark = 10;
	private int lowestHealth;

	//Helper variables
	private List<GameObject> nearbyObjects = new List<GameObject>();
	private List<GameObject> nearbyPlayers = new List<GameObject>();
	private bool isWeaponEquipped;


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
	//Movement
	public void SetMoveDirection(Vector2 moveInput)
	{
		moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
	}

	//Attack
	public void StartAttack()
	{
		if (weaponHand != null)
		{
			//TODO
			//weaponHand.StartAttack();
		}
	}
	public void PerformAttack()
	{
		if (weaponHand != null)
		{
			weaponHand.Attack();
			CancelAttack();
		}
	}
	public void CancelAttack()
	{
		if (weaponHand != null)
		{
			//TODO
			//weaponHand.CancelAttack();
		}
	}

	//Special attack
	public void StartSpecialAttack()
	{
		////TODO
		//if ((specialWeaponHand != null)
		//{
		//	specialWeaponHand.StartSpecialAttack();
		//}
	}
	public void PerformSpecialAttack()
	{
		////TODO
		//if ((specialWeaponHand != null)
		//{
		//	specialWeaponHand.PerformSpecialAttack();
		//}
	}
	public void CancelSpecialAttack()
	{
		////TODO
		//if ((specialWeaponHand != null)
		//{
		//	specialWeaponHand.CancelSpecialAttack();
		//}
	}

	//Throw
	public bool StartThrow()
	{
		if (weaponHand != null && isWeaponEquipped)
		{
			weaponHand.AimThrow();
			return true;
		}
		return false;
	}
	public bool PerformThrow()
	{
		if (weaponHand != null && isWeaponEquipped)
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
		if(addedThrowForce <= maxThrowForce)
		{
			addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
		}
	}

	//Pickup
	public void PerformPickup()
	{
		if (weaponHand != null && !isWeaponEquipped && nearbyObjects.Count > 0)
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

	//Revive
	public void Revive()
	{
		if(nearbyPlayers.Count > 0)
		{
			GameObject playerToHeal = null;
			lowestHealth = lowHeathMark;
			foreach (GameObject nearbyPlayer in nearbyPlayers)
			{
				if (nearbyPlayer.GetComponentInChildren<Attributes>().Health < lowestHealth)
				{
					playerToHeal = nearbyPlayer;
					lowestHealth = nearbyPlayer.GetComponentInChildren<Attributes>().Health;
				}
			}
			if(playerToHeal != null)
			{
				Debug.Log("Heal player " + playerToHeal.name);
			}
		}
	}


	public Quaternion CalculateRotation()
	{
		if(moveDirection != Vector3.zero)
		{
			rotationDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
		}
		return rotationDirection;
	}
	public void PerformRotation()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);
	}
	public Vector3 CalculateMovement()
	{
		Vector3 targetMoveAmount = moveDirection * moveSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
		return moveAmount;
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
