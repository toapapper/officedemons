using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>
/// Calculates and performs player movement
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-12
public class PlayerMovementController : MonoBehaviour
{
	private NavMeshAgent navmeshAgent;

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
		navmeshAgent = GetComponent<NavMeshAgent>();
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
		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position + moveAmount * Time.fixedDeltaTime);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		pos = Camera.main.ViewportToWorldPoint(pos);
		pos.y = Mathf.Clamp(pos.y, 0, 1.05f);

		navmeshAgent.Move(pos - transform.position);
	}
}
