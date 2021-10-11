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

	private bool isAddingThrowForce;
	private bool isAddingBombardForce;

	//Throwing variables
	[SerializeField]
	private float throwForceMultiplier = 25f;
	[SerializeField]
	private float maxThrowForce = 30f;
	private float addedThrowForce;

	//Bombard variables
	[SerializeField]
	private float bombardForceMultiplier = 5f;
	[SerializeField]
	private float maxBombardForce = 10f;
	private float addedBombardForce;


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
				}
				else if (context.action.name == inputControls.PlayerMovement.Attack.name)
				{
					if (player.CurrentState.IsActionTriggered && context.performed)
					{
						player.LockAction();
					}
					else if(weaponHand.objectInHand != null && weaponHand.objectInHand is BombardWeapon)
					{
						if (context.performed)
						{
							if (player.OnStartBombard())
							{
								playerMovement.MoveAmount = Vector3.zero;
								isAddingBombardForce = true;
							}
						}
						else if(context.canceled)
						{
							if (player.OnBombard())
							{
								isAddingBombardForce = false;
								addedBombardForce = 0;
							}
						}
					}
					else if (context.performed)
					{
						player.OnAttack();
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
							if (isAddingThrowForce)
							{
								addedThrowForce = 0;
								weaponHand.SetThrowForce(addedThrowForce);
							}
							else if (isAddingBombardForce)
							{
								addedBombardForce = 0;
								weaponHand.SetBombardForce(addedBombardForce);
							}
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
						if (context.started)
						{
							if (player.OnStartThrow())
							{
								playerMovement.MoveAmount = Vector3.zero;
								isAddingThrowForce = true;
							}
						}
						if (context.canceled)
						{
							if (player.OnThrow())
							{
								isAddingThrowForce = false;
								addedThrowForce = 0;
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
			else if (playerMovement.MoveAmount != Vector3.zero)
			{
				playerMovement.MoveAmount = Vector3.zero;
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
		else if (isAddingBombardForce)
		{
			if (addedBombardForce < maxBombardForce)
			{
				addedBombardForce += bombardForceMultiplier * Time.fixedDeltaTime;
				weaponHand.SetBombardForce(addedBombardForce);
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
}
