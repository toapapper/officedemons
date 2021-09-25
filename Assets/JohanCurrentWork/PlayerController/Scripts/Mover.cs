using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class Mover : MonoBehaviour
{
	//World transform variables
	private Vector3 forward;
	private Vector3 right;

	private CharacterController character;

	//Movement variables
	private Vector3 inputVector = Vector3.zero;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 moveAmount = Vector3.zero;
	private Vector3 smoothMoveVelocity;
	private Quaternion rotationDirection;

	//Movement speeds
	[SerializeField]
	private float moveSpeed = 1f;
	[SerializeField]
	private float rotationSpeed = 500f;
	

	private void Awake()
	{
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward.Normalize();
		right = new Vector3(forward.z, 0, -forward.x);

		character = GetComponent<CharacterController>();
	}

	public void SetInputVector(Vector2 direction)
	{
		inputVector = direction;
		moveDirection = (inputVector.x * right + inputVector.y * forward).normalized;
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
		////Throwing
		//if (isThrowing)
		//{
		//	weaponHand.AddThrowForce();
		//}
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

	//private void Update()
	//{
	//	//moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
	//	moveDirection = transform.TransformDirection(moveDirection);
	//	moveDirection *= moveSpeed;
	//	controller.Move(moveDirection * Time.deltaTime);
	//}
}
