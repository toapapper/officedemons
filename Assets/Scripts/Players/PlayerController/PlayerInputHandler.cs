using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
	private PlayerConfiguration playerConfiguration;
	private InputActions inputControls;

	private PlayerStateController player;
	private PlayerMovementController playerMovement;
	private WeaponHand weaponHand;
	//private WeaponHand weaponHand;

	//World transform variables
	private Vector3 forward;
	private Vector3 right;

	//Helper variables
	private List<GameObject> nearbyObjects = new List<GameObject>();
	private List<GameObject> nearbyPlayers = new List<GameObject>();

	public void Start()
	{
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward.Normalize();
		right = new Vector3(forward.z, 0, -forward.x);

		inputControls = new InputActions();
		player = GetComponent<PlayerStateController>();
		playerMovement = GetComponent<PlayerMovementController>();
		weaponHand = GetComponent<WeaponHand>();
	}

	public void InitializePlayer(PlayerConfiguration pc)
	{
		playerConfiguration = pc;
		playerConfiguration.Input.onActionTriggered += Input_onActionTriggered;
	}

	private void Input_onActionTriggered(CallbackContext context)
	{
		if (player != null)
		{
			if (!player.CurrentState.IsActionLocked)
			{
				if (context.action.name == inputControls.PlayerMovement.Move.name)
				{
					Vector2 moveInput = context.ReadValue<Vector2>();
					playerMovement.MoveDirection = (moveInput.x * right + moveInput.y * forward).normalized;



					//Vector2 moveInput = context.ReadValue<Vector2>();

					//Vector3 direction = (context.ReadValue<Vector2>().x * right + context.ReadValue<Vector2>().y * forward).normalized;
					//player.RotationDirection = Quaternion.LookRotation(direction, Vector3.up);
					//if (!player.CurrentState.IsActionTriggered && !player.CurrentState.IsStaminaDepleted)
					//{
					//	playerMovement.MoveDirection = direction;
					//}
					//else
					//{
					//	playerMovement.MoveDirection = Vector3.zero;
					//}

					//Quaternion rotationDirection = Quaternion.LookRotation(direction, Vector3.up);
					//playerMovement.MoveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
					//if (!player.CurrentState.IsActionTriggered/* && !player.CurrentState.IsStaminaDepleted*/)
					//{
					//	Vector2 moveInput = context.ReadValue<Vector2>();
					//	playerMovement.MoveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
					//	//playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
					//}
					//else
					//{
					//	playerMovement.MoveDirection = Vector3.zero;
					//}
				}
				else if (context.action.name == inputControls.PlayerMovement.Attack.name)
				{
					if (context.performed)
					{
						if (!player.CurrentState.IsActionTriggered)
						{
							player.OnAttack();
						}
						else
						{
							player.LockAction();
						}
							
					}
				}
				else if (context.action.name == inputControls.PlayerMovement.Special.name)
				{
					if (context.performed)
					{
						if (!player.CurrentState.IsActionTriggered)
						{
							player.OnSpecial();
						}
						else
						{
							player.CancelAction();
						}
					}
				}
				else if (context.action.name == inputControls.PlayerMovement.PickUp.name)
				{
					if (weaponHand.objectInHand == null)
					{
						if (context.canceled && nearbyObjects.Count > 0)
						{
							foreach (GameObject nearbyObject in nearbyObjects)
							{
								if (!nearbyObject.GetComponentInChildren<AbstractWeapon>().IsHeld)
								{
									player.OnPickUp(nearbyObject);
									break;
								}
							}
						}
					}
					else
					{
						player.OnThrow(context);
					}
					//	else
					//	{
					//		if (context.started)
					//		{
					//			player.StartThrow();
					//		}
					//		else if (context.canceled)
					//		{
					//			player.Throw(addedThrowForce);
					//			addedThrowForce = 0f;
					//		}
					//	}


					//player.OnPickupThrow(context);
				}
				else if (context.action.name == inputControls.PlayerMovement.Revive.name)
				{
					player.OnRevive(context);
				}
				//else if (context.action.name == inputControls.PlayerMovement.PickUp.name)
				//{
				//	if (weaponHand.objectInHand == null)
				//	{
				//		if (context.canceled && nearbyObjects.Count > 0)
				//		{
				//			foreach (GameObject nearbyObject in nearbyObjects)
				//			{
				//				if (!nearbyObject.GetComponentInChildren<AbstractWeapon>().IsHeld)
				//				{
				//					player.PickUp(nearbyObject);
				//					break;
				//				}
				//			}
				//		}
				//	}
				//	else
				//	{
				//		if (context.started)
				//		{
				//			player.StartThrow();
				//		}
				//		else if (context.canceled)
				//		{
				//			player.Throw(addedThrowForce);
				//			addedThrowForce = 0f;
				//		}
				//	}
				//}
				//else if (context.action.name == inputControls.PlayerMovement.Revive.name)
				//{
				//	if (context.performed)
				//	{
				//		player.OnRevive();
				//	}
				//}
			}
			else if (playerMovement.MoveDirection != Vector3.zero)
			{
				playerMovement.MoveDirection = Vector3.zero;
			}
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "WeaponObject")
		{
			nearbyObjects.Add(other.gameObject);
		}
		else if (other.gameObject.tag == "Player")
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
		else if (other.gameObject.tag == "Player")
		{
			nearbyPlayers.Remove(other.gameObject);
		}
	}
}
