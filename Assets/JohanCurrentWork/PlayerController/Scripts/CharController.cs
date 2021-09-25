using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
	//World transform variables
	private Vector3 forward;
	private Vector3 right;

	//Character movers
	private CharacterController character;
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

	//Helper variables
	private List<GameObject> nearbyObjects = new List<GameObject>();
	private bool isweaponEquipped;
	private bool isActionStarted;
	private bool isAttack, isSpecialAttack, isThrow, isAddingThrowForce;



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

	//Move
	public void SetMoveDirection(Vector2 direction)
	{
		moveDirection = (direction.x * right + direction.y * forward).normalized;
	}

	//CombatActions
	public void AcceptCombatAction()
	{
		//TODO:
		//End Turn
	}
	public void PerformCombatAction()
	{
		if (isAttack)
		{
			PerformAttack();
		}
		else if (isSpecialAttack)
		{
			PerformSpecialAttack();
		}
		else if (isThrow)
		{
			PerformThrow();
		}
	}
	public void CancelCombatAction()
	{
		if (isAttack)
		{
			CancelAttack();
		}
		else if (isSpecialAttack)
		{
			CancelSpecialAttack();
		}
		else if (isThrow)
		{
			CancelThrow();
		}
	}

	//Attack
	public void StartAttack()
	{
		if (weaponHand != null && !isActionStarted)
		{
			isActionStarted = true;
			isAttack = true;
			//TODO
			//weaponHand.StartHit();
		}
	}
	public void PerformAttack()
	{
		if (weaponHand != null && isAttack)
		{
			weaponHand.Attack();
			CancelAttack();
		}
	}
	public void CancelAttack()
	{
		//TODO
		//weaponHand.CancelHit();
		isAttack = false;
		isActionStarted = false;
	}

	//Special attack
	public void StartSpecialAttack()
	{
		if (!isActionStarted)
		{
			isActionStarted = true;
			isSpecialAttack = true;
			//TODO
			//specialWeaponHand.StartSpecialAttack();
		}
	}
	public void PerformSpecialAttack()
	{
		if (isSpecialAttack)
		{
			//TODO
			//if (specialHand != null)
			//{
			//	specialHand.Attack();
			//}
			CancelSpecialAttack();
		}
	}
	public void CancelSpecialAttack()
	{
		//TODO
		//specialWeaponHand.CancelSpecialAttack();
		isSpecialAttack = false;
		isActionStarted = false;
	}

	//Throw
	public void StartThrow()
	{
		if (weaponHand != null && isweaponEquipped && !isActionStarted)
		{
			isActionStarted = true;
			isThrow = true;
			isAddingThrowForce = true;
			weaponHand.AimThrow();
		}
	}
	public void StopAddingThrowForce()
	{
		isAddingThrowForce = false;
	}
	public void PerformThrow()
	{
		if (weaponHand != null && isweaponEquipped && isThrow)
		{
			weaponHand.Throw(addedThrowForce);
			isweaponEquipped = false;
			CancelThrow();
		}
	}
	public void CancelThrow()
	{
		StopAddingThrowForce();
		addedThrowForce = 0;
		isThrow = false;
		isActionStarted = false;
	}

	//Pick up
	public void PerformPickup()
	{
		if (weaponHand != null && !isweaponEquipped && nearbyObjects.Count > 0)
		{
			foreach (GameObject neabyObject in nearbyObjects)
			{
				if (!neabyObject.GetComponentInChildren<AbstractWeapon>().IsHeld)
				{
					weaponHand.Equip(neabyObject);
					isweaponEquipped = true;
					break;
				}
			}
		}
	}


	private void FixedUpdate()
	{
		//Rotation
		if (moveDirection != Vector3.zero)
		{
			CalculateRotation();
		}
		if (transform.rotation != rotationDirection)
		{
			PerformRotation();
		}
		if (isActionStarted)
		{
			//Throwing
			if (isAddingThrowForce && addedThrowForce <= maxThrowForce)
			{
				addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
			}
		}
		//Movement
		else
		{
			CalculateMovement();
			if (moveAmount != Vector3.zero)
			{
				PerformMovement();
			}
		}
	}

	private void CalculateRotation()
	{
		rotationDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
	}
	private void PerformRotation()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);
	}
	private void CalculateMovement()
	{
		Vector3 targetMoveAmount = moveDirection * moveSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
		if (transform.position.y > 0)
		{
			character.Move(Vector3.down);
		}
	}
	private void PerformMovement()
	{
		Vector3 localMove = moveAmount * Time.fixedDeltaTime;
		character.Move(localMove);
	}



	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "WeaponObject")
		{
			nearbyObjects.Add(other.gameObject);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "WeaponObject")
		{
			nearbyObjects.Remove(other.gameObject);
		}
	}
}
