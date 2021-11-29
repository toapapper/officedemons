using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// <para>
/// Handle player input and sends information to state controller
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-11-26
public class PlayerInputHandler : MonoBehaviour
{
	private PlayerConfiguration playerConfiguration;
	private InputActions inputControls;

	private PlayerStateController player;
	private PlayerMovementController playerMovement;
	private WeaponHand weaponHand;
	private SpecialHand specialHand;

	//World transform variables
	private Vector3 forward;
	private Vector3 right;

	//Helper variables
	private List<GameObject> nearbyObjects = new List<GameObject>();
	private List<GameObject> nearbyPlayers = new List<GameObject>();

	public bool recentlySpawned = false;
	private bool isAddingThrowForce;

	//Throwing variables
	[SerializeField]
	private float throwForceMultiplier = 25f;
	[SerializeField]
	private float maxThrowForce = 30f;
	private float addedThrowForce;


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
		specialHand = GetComponent<SpecialHand>();

		//if you spawned while the game was running this will be set to true.
		//It's here just in order to put you into the proper state, if for example you where to spawn in combat.
		if (recentlySpawned)
        {
			GetComponent<PlayerStateController>().Revive();
			recentlySpawned = false;
        }
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
				if (player.CurrentState.IsAddingBombardForce)
				{
					switch (player.CurrentState.ChosenAction)
					{
						case TypeOfAction.BOMBARD:
							if (context.action.name == inputControls.PlayerMovement.Move.name)
							{
								Vector2 moveInput = context.ReadValue<Vector2>();
								weaponHand.ThrowAim.TargetDirection = (moveInput.x * right + moveInput.y * forward).normalized;
							}
							else if (context.performed)
							{
								if (context.action.name == inputControls.PlayerMovement.Attack.name)
								{
									player.LockAction();
								}
								else
								{
									player.CancelAction();
								}
								player.CurrentState.IsAddingBombardForce = false;
								playerMovement.MoveDirection = Vector3.zero;
								playerMovement.MoveAmount = Vector3.zero;
							}
							break;
						case TypeOfAction.SPECIALBOMBARD:
							if (context.action.name == inputControls.PlayerMovement.Move.name)
							{
								Vector2 moveInput = context.ReadValue<Vector2>();
								specialHand.ThrowAim.TargetDirection = (moveInput.x * right + moveInput.y * forward).normalized;
							}
							else if (context.performed)
							{
								if (context.action.name == inputControls.PlayerMovement.Special.name)
								{
									player.LockAction();
								}
								else
								{
									player.CancelAction();
								}
								player.CurrentState.IsAddingBombardForce = false;
								playerMovement.MoveDirection = Vector3.zero;
								playerMovement.MoveAmount = Vector3.zero;
							}
							break;
					}
				}
				else if (context.action.name == inputControls.PlayerMovement.Move.name)
				{
					Vector2 moveInput = context.ReadValue<Vector2>();
					playerMovement.MoveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
				}
				else if (isAddingThrowForce)
				{
					if (context.canceled)
					{
						if (player.OnThrow())
						{
							isAddingThrowForce = false;
							//player.CurrentState.IsActionTriggered = false;
							addedThrowForce = 0;
						}
					}
				}
				else if (!player.CurrentState.IsActionTriggered)
				{
					if (context.action.name == inputControls.PlayerMovement.Attack.name)
					{
						if (weaponHand.objectInHand != null && weaponHand.objectInHand is BombardWeapon)
						{
							if (context.performed)
							{
								if (player.OnStartBombard())
								{
									playerMovement.MoveAmount = Vector3.zero;
									player.CurrentState.IsAddingBombardForce = true;
								}
							}
							//else if (context.canceled)
							//{
							//	if (player.OnBombard())
							//	{
							//		player.CurrentState.IsAddingBombardForce = false;
							//	}
							//}
						}
						else if (context.performed)
						{
							player.OnAttack();
						}
					}
					else if (context.action.name == inputControls.PlayerMovement.Special.name && specialHand.ObjectInHand.Charges > 0)
					{
						if(specialHand.ObjectInHand is CoffeeCupSpecial)
						{
							if (context.performed)
							{
								if (player.OnStartSpecialBombard())
								{
									playerMovement.MoveAmount = Vector3.zero;
									player.CurrentState.IsAddingBombardForce = true;
								}
							}
							//else if (context.canceled)
							//{
							//	if (player.OnSpecialBombard())
							//	{
							//		player.CurrentState.IsAddingBombardForce = false;
							//	}
							//}
						}
						else if (context.performed)
						{
							player.OnSpecial();
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
						else if(!isAddingThrowForce)
						{
							if (context.started)
							{
								if (player.OnStartThrow())
								{
									playerMovement.MoveAmount = Vector3.zero;
									isAddingThrowForce = true;
								}
							}
						}
					}
					else if (context.action.name == inputControls.PlayerMovement.Revive.name)
					{
						if (context.performed)
						{
							Debug.Log(name);
							Debug.Log("Revive " + nearbyPlayers.Count);
							if (nearbyPlayers.Count > 0)
							{
								foreach (GameObject nearbyPlayer in nearbyPlayers)
								{
									//if (nearbyPlayer.GetComponentInChildren<AbstractPlayerState>() is DeadState)
									if (nearbyPlayer.GetComponentInChildren<Attributes>().Health <= 0)
									{
										Debug.Log("Player name " + nearbyPlayer.ToString());
										player.OnRevive(nearbyPlayer);
										return;
									}
								}
							}
						}
					}
				}
				else if (context.performed)
				{
					switch(player.CurrentState.ChosenAction)
					{
						case TypeOfAction.ATTACK:
						case TypeOfAction.BOMBARD:
							if (context.action.name == inputControls.PlayerMovement.Attack.name)
							{
								player.LockAction();
							}
							else
							{
								player.CancelAction();
							}
							break;
						case TypeOfAction.SPECIALATTACK:
						case TypeOfAction.SPECIALBOMBARD:
							if (context.action.name == inputControls.PlayerMovement.Special.name)
							{
								player.LockAction();
							}
							else
							{
								player.CancelAction();
							}
							break;
						case TypeOfAction.THROW:
							if (context.action.name == inputControls.PlayerMovement.PickUp.name)
							{
								player.LockAction();
							}
							else
							{
								player.CancelAction();
							}
							break;
						case TypeOfAction.REVIVE:
							if (context.action.name == inputControls.PlayerMovement.Revive.name)
							{
								player.LockAction();
							}
							else
							{
								player.CancelAction();
							}
							break;
					}
					playerMovement.MoveDirection = Vector3.zero;
					playerMovement.MoveAmount = Vector3.zero;
				}
			}
			else if (playerMovement.MoveAmount != Vector3.zero)
			{
				playerMovement.MoveDirection = Vector3.zero;
				playerMovement.MoveAmount = Vector3.zero;
			}
			
			if (context.action.name == inputControls.PlayerMovement.Pause.name && context.performed)
			{
				GameManager.Instance.OnPause();
			}
		}
	}

	private void FixedUpdate()
	{
		if (isAddingThrowForce)
		{
			if (addedThrowForce < maxThrowForce)
			{
				addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
				weaponHand.SetThrowForce(addedThrowForce);
			}
		}
	}

	

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "WeaponObject")
		{
			if (!nearbyObjects.Contains(other.gameObject))
				nearbyObjects.Add(other.gameObject);
		}
		else if (other.gameObject.tag == "Player")
		{
			if (!nearbyPlayers.Contains(other.gameObject) && other != gameObject)
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

	public void RemoveObjectFromWeaponList(GameObject weapon)
	{
		nearbyObjects.Remove(weapon);
	}
}
