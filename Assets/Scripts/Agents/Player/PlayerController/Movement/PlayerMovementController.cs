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
	private float slowEffect = 1;
	private float moveTime;


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
	public float SlowEffect
	{
		get { return slowEffect; }
		set { slowEffect = value; }
	}
	public Quaternion RotationDirection
	{
		get { return rotationDirection; }
		set { rotationDirection = value; }
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
			//Debug.Log("CALCULATES ROTATION");
			rotationDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
		}
		return rotationDirection;
	}
	public Vector3 CalculateMovement()
	{
		Vector3 targetMoveAmount = moveDirection * moveSpeed * slowEffect;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
		return moveAmount;
	}

	//Perform movement
	public void PerformRotation()
	{
		//Debug.Log("PERFORMS ROTATION");
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

	/// <summary>
	/// Move to position
	/// </summary>
	/// <param name="pos"></param>
	public void MoveTo(Vector3 pos)
	{
		navmeshAgent.SetDestination(pos);
		moveTime = 10f;
	}

	/// <summary>
	/// Checks if players have reached position
	/// </summary>
	/// <returns></returns>
	public bool AtDestination()
	{
		moveTime -= Time.deltaTime;
		if(moveTime <= 0)
		{
			transform.position = navmeshAgent.destination;
			navmeshAgent.ResetPath();
			return true;
		}
		if (!navmeshAgent.pathPending)
		{
			if (navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
			{
				if (!navmeshAgent.hasPath || navmeshAgent.velocity.sqrMagnitude == 0f)
				{
					navmeshAgent.ResetPath();
					return true;
				}
			}
		}
		return false;
	}

	public void ResetNavMeshPath()
    {
		navmeshAgent.ResetPath();
	}
}
