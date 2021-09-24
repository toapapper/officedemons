using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 3f;

	private CharacterController controller;

	private Vector3 moveDirection = Vector3.zero;
	private Vector3 inputVector = Vector3.zero;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	public void SetInputVector(Vector2 direction)
	{
		inputVector = direction;
	}
	public void SetAttack(float attackInput)
	{

	}
	public void SetSpecial(float specialInput)
	{

	}
	public void SetThrow(float throwInput)
	{

	}
	public void SetPickup(float pickupInput)
	{

	}


	private void Update()
	{
		moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= moveSpeed;
		controller.Move(moveDirection * Time.deltaTime);
	}
}
