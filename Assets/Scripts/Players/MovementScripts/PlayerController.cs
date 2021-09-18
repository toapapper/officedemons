using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class PlayerController : MonoBehaviour
{
	//World transform
	private GameObject worldCenter;
	private Vector3 forward;
	private Vector3 right;

	//Inputs
	private Vector2 moveInput;
	private float pickupInput; 
	private float attackInput;
	private float specialInput;
	private float throwInput;

	//Playermovement
	[SerializeField]
	private WeaponHand weaponHand;
	CharacterController character;
	private Vector3 moveDirection;
	private Vector3 moveAmount = Vector3.zero;
	private Vector3 smoothMoveVelocity;
	private Quaternion rotationDirection;
	[SerializeField]
	private float speed = 1f; //Movement speed
	[SerializeField]
	private float rotationSpeed = 500f; // Rotation speed

	//Helper variables
	private int playerNr;
	private List<GameObject> nearbyObjects = new List<GameObject>();
	private bool weaponEquipped;
	

	void OnEnable()
	{
		if (PlayerManager.players == null)
			PlayerManager.players = new List<GameObject>();

		PlayerManager.players.Add(this.gameObject);
		playerNr = PlayerManager.players.Count;
	}

	private void Start()
	{
		EnterGame();
	}

	private void EnterGame()
	{
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward.Normalize();
		right = new Vector3(forward.z, 0, -forward.x);

		worldCenter = GameObject.Find("WorldCenter");
		transform.position = worldCenter.transform.position + new Vector3(0,0,playerNr);

		character = GetComponent<CharacterController>();
	}


	public void OnMove(InputAction.CallbackContext context)
	{
		if (name != "Player")
		{
			moveInput = context.ReadValue<Vector2>();
			moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
		}
	}
	public void OnPickUp(InputAction.CallbackContext context)
	{
		if(name != "Player" && context.performed)
		{
			if (!weaponEquipped)
			{
				if (nearbyObjects.Count > 0)
				{
					foreach(GameObject neabyObject in nearbyObjects)
					{
						if (!neabyObject.GetComponent<Weapon>().isHeld)
						{
							weaponHand.Equip(neabyObject);
							weaponEquipped = true;
							break;
						}
					}
				}
			}
			else
			{
				weaponHand.DropObject();
				weaponEquipped = false;
			}
			
		}		
	}
	public void OnAttack(InputAction.CallbackContext context)
	{
		if (name != "Player" && context.performed)
		{
			weaponHand.Hit();
		}			
	}
	public void OnSpeciel(InputAction.CallbackContext context)
	{
		if (name != "Player" && context.performed)
		{
			specialInput = context.ReadValue<float>();
			Debug.Log("Special" + specialInput);
		}
	}
	public void OnThrow(InputAction.CallbackContext context)
	{
		if (name != "Player" && context.performed)
		{
			if (weaponEquipped)
			{
				weaponHand.Throw(transform.forward);
				weaponEquipped = false;
			}
		}
	}


	private void FixedUpdate()
	{
		if (moveDirection != Vector3.zero)
		{
			CalculateRotation();
		}
		if(transform.rotation != rotationDirection)
		{
			PerformRotation();
		}

		CalculateMovement();
		if(moveAmount != Vector3.zero)
		{
			PerformMovement();
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
		Vector3 targetMoveAmount = moveDirection * speed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
		if(transform.position.y > 0)
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
