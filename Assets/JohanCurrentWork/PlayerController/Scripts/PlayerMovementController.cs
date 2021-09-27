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

	//Helper variables
	private List<GameObject> nearbyObjects = new List<GameObject>();
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

	public void SetMoveDirection(Vector2 moveInput)
	{
		moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
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
	public bool StartThrow()
	{
		if (weaponHand != null && isWeaponEquipped && !isActionStarted)
		{
			isActionStarted = true;
			isThrow = true;
			isAddingThrowForce = true;
			weaponHand.AimThrow();
			return true;
		}
		return false;
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
		//TODO
		//weaponHand.CancelThrow();
		StopAddingThrowForce();
		addedThrowForce = 0;
		isThrow = false;
		isActionStarted = false;
	}
	public void StopAddingThrowForce()
	{
		isAddingThrowForce = false;
	}

	//Pick up
	public void PerformPickup()
	{
		if (weaponHand != null && !isWeaponEquipped && nearbyObjects.Count > 0)
		{
			foreach (GameObject neabyObject in nearbyObjects)
			{
				if (!neabyObject.GetComponentInChildren<AbstractWeapon>().IsHeld)
				{
					weaponHand.Equip(neabyObject);
					isWeaponEquipped = true;
					break;
				}
			}
		}
	}


	public Quaternion CalculateRotation()
	{
		rotationDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
		return rotationDirection;
	}
	public void PerformRotation()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed * Time.fixedDeltaTime);
	}
	public Vector3 CalculateMovement()
	{
		//moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
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
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "WeaponObject")
		{
			nearbyObjects.Remove(other.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "WeaponObject")
		{
			Physics.IgnoreCollision(character, collision.collider);
		}
	}
}
