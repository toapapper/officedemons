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
	//private WeaponHand weaponHand;

	//Helper variables
	//private List<GameObject> nearbyObjects = new List<GameObject>();
	//private List<GameObject> nearbyPlayers = new List<GameObject>();

	public void Start()
	{
		inputControls = new InputActions();
		player = GetComponent<PlayerStateController>();
		playerMovement = GetComponent<PlayerMovementController>();
		//weaponHand = GetComponent<WeaponHand>();
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
					playerMovement.SetMoveDirection(context.ReadValue<Vector2>());
				}
				else if (context.action.name == inputControls.PlayerMovement.Attack.name)
				{
					if (context.performed)
					{
						player.OnAttack();
					}
				}
				else if (context.action.name == inputControls.PlayerMovement.Special.name)
				{
					if (context.performed)
					{
						player.OnSpecial();
					}
				}
				else if (context.action.name == inputControls.PlayerMovement.PickUp.name)
				{
					player.OnPickupThrow(context);
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


	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.gameObject.tag == "WeaponObject")
	//	{
	//		nearbyObjects.Add(other.gameObject);
	//	}
	//	else if (other.gameObject.tag == "Player")
	//	{
	//		nearbyPlayers.Add(other.gameObject);
	//	}
	//}
	//private void OnTriggerExit(Collider other)
	//{
	//	if (other.gameObject.tag == "WeaponObject")
	//	{
	//		nearbyObjects.Remove(other.gameObject);
	//	}
	//	else if (other.gameObject.tag == "Player")
	//	{
	//		nearbyPlayers.Remove(other.gameObject);
	//	}
	//}
}
