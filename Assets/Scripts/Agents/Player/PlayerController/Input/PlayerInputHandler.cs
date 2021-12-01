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

	private TypeOfAction chosenAction;
	private bool isInputLocked;
	private bool isInputTriggered;
	private bool isBombarding;
	private bool isThrowing;

	public TypeOfAction ChosenAction { get { return chosenAction; } }
	public bool IsInputLocked {	get { return isInputLocked; } }
	public bool IsInputTriggered { get { return isInputTriggered; } }


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

		chosenAction = TypeOfAction.NOACTION;
	}

	public void InitializePlayer(PlayerConfiguration pc)
	{
		playerConfiguration = pc;
		playerConfiguration.Input.onActionTriggered += Input_onActionTriggered;
	}

	#region INPUTEVENT
	private void Input_onActionTriggered(CallbackContext context)
	{
		if (player != null)
		{
			if (context.action.name == inputControls.PlayerMovement.Pause.name && context.performed)
			{
				GameManager.Instance.OnPause();
			}
			else if (!isInputLocked)
			{
				if (context.action.name == inputControls.PlayerMovement.Move.name)
				{
					InputMove(context);
				}
				else if (!isInputTriggered)
				{
					if (context.performed)
					{
						if (context.action.name == inputControls.PlayerMovement.Attack.name)
						{
							InpuAttack(context);
						}
						else if (context.action.name == inputControls.PlayerMovement.Special.name)
						{
							InputSpecial(context);
						}
						else if (context.action.name == inputControls.PlayerMovement.PickUp.name)
						{
							InputPickupThrow(context);
						}
						else if (context.action.name == inputControls.PlayerMovement.Revive.name)
						{
							InputRevive(context);
						}
					}
				}
				else
				{
					ConfirmInput(context);
				}
			}
			else if (playerMovement.MoveAmount != Vector3.zero)
			{
				playerMovement.MoveDirection = Vector3.zero;
				playerMovement.MoveAmount = Vector3.zero;
			}
		}
	}
	#endregion

	#region INPUTS

	#region MOVE
	private void InputMove(CallbackContext context)
	{
		Vector2 moveInput = context.ReadValue<Vector2>();
		if (!isBombarding)
		{
			playerMovement.MoveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
		}
		else
		{
			switch (chosenAction)
			{
				case TypeOfAction.BOMBARD:
					weaponHand.ThrowAim.TargetDirection = (moveInput.x * right + moveInput.y * forward).normalized;
					break;
				case TypeOfAction.SPECIALBOMBARD:
					specialHand.ThrowAim.TargetDirection = (moveInput.x * right + moveInput.y * forward).normalized; playerMovement.MoveAmount = Vector3.zero;
					break;
			}
		}
	}
	#endregion

	#region ATTACK
	private void InpuAttack(CallbackContext context)
	{
		isInputTriggered = true;
		playerMovement.MoveDirection = Vector3.zero;
		playerMovement.MoveAmount = Vector3.zero;

		if (weaponHand.objectInHand != null && weaponHand.objectInHand is BombardWeapon)
		{
			chosenAction = TypeOfAction.BOMBARD;
			isBombarding = true;
			player.OnStartBombard();
		}
		else
		{
			chosenAction = TypeOfAction.ATTACK;
			player.OnAttack();
		}
	}
	#endregion

	#region SPECIAL
	private void InputSpecial(CallbackContext context)
	{
		if (specialHand.ObjectInHand.Charges > 0)
		{
			isInputTriggered = true;
			playerMovement.MoveDirection = Vector3.zero;
			playerMovement.MoveAmount = Vector3.zero;

			if (specialHand.ObjectInHand is CoffeeCupSpecial)
			{
				chosenAction = TypeOfAction.SPECIALBOMBARD;
				isBombarding = true;
				player.OnStartSpecialBombard();
			}
			else
			{
				chosenAction = TypeOfAction.SPECIALATTACK;
				player.OnSpecial();
			}
		}
	}
	#endregion

	#region PICKUP/THROW
	private void InputPickupThrow(CallbackContext context)
	{
		if (weaponHand.objectInHand == null)
		{
			if (nearbyObjects.Count > 0)
			{
				foreach (GameObject nearbyObject in nearbyObjects)
				{
					if (!nearbyObject.GetComponentInChildren<AbstractWeapon>().IsHeld)
					{
						isInputTriggered = true;
						playerMovement.MoveAmount = Vector3.zero;
						playerMovement.MoveDirection = Vector3.zero;

						chosenAction = TypeOfAction.PICKUP;
						player.OnPickUp(nearbyObject);
						break;
					}
				}
			}
		}
		else
		{
			isInputTriggered = true;
			playerMovement.MoveAmount = Vector3.zero;
			playerMovement.MoveDirection = Vector3.zero;

			chosenAction = TypeOfAction.THROW;
			isThrowing = true;
			player.OnStartThrow();
		}
	}
	#endregion

	#region REVIVE
	private void InputRevive(CallbackContext context)
	{
		if (nearbyPlayers.Count > 0)
		{
			foreach (GameObject nearbyPlayer in nearbyPlayers)
			{
				//if (nearbyPlayer.GetComponentInChildren<AbstractPlayerState>() is DeadState)
				if (nearbyPlayer.GetComponentInChildren<Attributes>().Health <= 0)
				{
					isInputTriggered = true;
					playerMovement.MoveAmount = Vector3.zero;
					playerMovement.MoveDirection = Vector3.zero;

					chosenAction = TypeOfAction.REVIVE;
					player.OnRevive(nearbyPlayer);
					return;
				}
			}
		}
	}
	#endregion

	#region CONFIRM
	private void ConfirmInput(CallbackContext context)
	{
		if (isThrowing)
		{
			if (context.canceled)
			{
				isInputLocked = true;
				isThrowing = false;
				addedThrowForce = 0;
				player.OnThrow();
			}
		}
		else if (context.performed)
		{
			switch (chosenAction)
			{
				case TypeOfAction.BOMBARD:
				case TypeOfAction.ATTACK:
					if (context.action.name == inputControls.PlayerMovement.Attack.name)
					{
						confirmed = true;
						isInputLocked = true;
						player.LockAction();
					}
					else
					{
						player.CancelAction();
						ResetInput();
					}
					isBombarding = false;
					break;

				case TypeOfAction.SPECIALBOMBARD:
				case TypeOfAction.SPECIALATTACK:
					if (context.action.name == inputControls.PlayerMovement.Special.name)
					{
						isInputLocked = true;
						player.LockAction();
					}
					else
					{
						player.CancelAction();
						ResetInput();
					}
					isBombarding = false;
					break;

				case TypeOfAction.REVIVE:
					if (context.action.name == inputControls.PlayerMovement.Revive.name)
					{
						isInputLocked = true;
						player.LockAction();
					}
					else
					{
						player.CancelAction();
						ResetInput();
					}
					break;
			}
			playerMovement.MoveDirection = Vector3.zero;
			playerMovement.MoveAmount = Vector3.zero;
		}
	}
	#endregion

	#endregion

	#region SET/RESET
	public void LockInput()
	{
		isInputLocked = true;
	}
	public void ResetInput()
	{
		chosenAction = TypeOfAction.NOACTION;
		isInputLocked = false;
		isInputTriggered = false;
		isBombarding = false;
		isThrowing = false;
	}
	#endregion

	#region UPDATE
	private void FixedUpdate()
	{
		if (!isInputLocked)
		{
			if (isThrowing)
			{
				if (addedThrowForce < maxThrowForce)
				{
					addedThrowForce += throwForceMultiplier * Time.fixedDeltaTime;
					weaponHand.SetThrowForce(addedThrowForce);
				}
			}
			if (!isBombarding)
			{
				//Rotation
				if (playerMovement.CalculateRotation() != transform.rotation)
				{
					playerMovement.PerformRotation();
				}
				if (!isInputTriggered)
				{
					//Movement
					if (playerMovement.CalculateMovement() != Vector3.zero)
					{
						playerMovement.PerformMovement();
					}
				}
			}

			//player.OnUpdateState();
		}
	}
	#endregion

	#region NEARBYHANDLER
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

	public void ClearNearbyObjectList()
	{
		nearbyObjects.Clear();
	}
	#endregion
}
