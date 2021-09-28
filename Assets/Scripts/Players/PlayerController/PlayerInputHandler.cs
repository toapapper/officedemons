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

	private AbstractPlayerState player;


	public void Start()
	{
		inputControls = new InputActions();
		player = GetComponent<AbstractPlayerState>();
	}

	public void InitializePlayer(PlayerConfiguration pc)
	{
		playerConfiguration = pc;
		playerConfiguration.Input.onActionTriggered += Input_onActionTriggered;
	}

	private void Input_onActionTriggered(CallbackContext obj)
	{
		if (player != null)
		{
			if (obj.action.name == inputControls.PlayerMovement.Move.name)
			{
				player.OnMove(obj);
			}
			else if (obj.action.name == inputControls.PlayerMovement.Attack.name)
			{
				player.OnAttack(obj);
			}
			else if (obj.action.name == inputControls.PlayerMovement.Special.name)
			{
				player.OnSpecial(obj);
			}
			//else if (obj.action.name == inputControls.PlayerMovement.Throw.name)
			//{
			//	player.OnThrow(obj);
			//}
			else if (obj.action.name == inputControls.PlayerMovement.PickUp.name)
			{
				player.OnPickupThrow(obj);
			}
			//else if(obj.action.name == inputControls.PlayerMovement.Revive.name)
			//{
			//	player.OnRevive(obj);
			//}
		}
	}
}
