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
	CharacterController character;
	private Vector3 moveDirection;
	private Vector3 moveAmount = Vector3.zero;
	private Vector3 smoothMoveVelocity;
	private Quaternion rotationDirection;
	[SerializeField]
	private float speed = 1f; //Movement speed
	[SerializeField]
	private float rotationSpeed = 500f; // Rotation speed
	[SerializeField]
	private float controllerSensitivity = 0.5f; // Rotation speed

	//Helper variables
	private int playerNr;
	private bool objectNearby;

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
	
	public void OnKeyboardMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
		moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
	}
	public void OnControllerMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
		if (moveInput.magnitude > controllerSensitivity)
		{
			moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
		}
		else
		{
			moveDirection = Vector3.zero;
		}
	}
	public void OnPickUp(InputAction.CallbackContext context)
	{
		if (objectNearby)
		{
			pickupInput = context.ReadValue<float>();
			Debug.Log("Pick up" + pickupInput);
		}
		else
		{
			Debug.Log("Throw" + pickupInput);
		}
		
	}
	public void OnAttack(InputAction.CallbackContext context)
	{
		attackInput = context.ReadValue<float>();
		Debug.Log("Attack" + attackInput);
	}
	public void OnSpeciel(InputAction.CallbackContext context)
	{
		specialInput = context.ReadValue<float>();
		Debug.Log("Special" + specialInput);
	}
	public void OnThrow(InputAction.CallbackContext context)
	{
		throwInput = context.ReadValue<float>();
		Debug.Log("Special" + throwInput);
	}

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.canceled && transform.name != "Player")
        {
            GameManager.Instance.OnPause();
            Debug.Log("Pause");
            Debug.Log(this.ToString() + " " + this.transform.position);
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
	}
	private void PerformMovement()
	{
		Vector3 localMove = moveAmount * Time.fixedDeltaTime;
		character.Move(localMove);
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "WeaponObject")
		{

		}
	}
}
