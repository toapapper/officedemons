using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
//public enum SelctetAction
//{
//	None,
//	Attack,
//	SpecialAttack,
//	Throw
//}

public class PlayerInputHandler : MonoBehaviour
{
	private PlayerConfiguration playerConfiguration;
	private InputActions inputControls;

	private AbstractPlayerState player;

	//Character movers
	//private CharController playerController;

	//private bool inCombat, activeTurn, selectedAction;

	//public bool InCombat
	//{
	//	get { return inCombat; }
	//	set { inCombat = value; }
	//}
	//public bool ActiveTurn
	//{
	//	get { return activeTurn; }
	//	set { activeTurn = value; }
	//}

	private void Awake()
	{
		inputControls = new InputActions();
		//playerController = GetComponent<CharController>();
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
			else if (obj.action.name == inputControls.PlayerMovement.Throw.name)
			{
				player.OnThrow(obj);
			}
			else if (obj.action.name == inputControls.PlayerMovement.PickUp.name)
			{
				player.OnPickup(obj);
			}
		}
	}

	//public void EndTurn()
	//{
	//	if (selectedAction)
	//	{
	//		playerController.AcceptCombatAction();
	//	}
	//	activeTurn = false;
	//}
	//public void PerformCombatAction()
	//{
	//	if (!activeTurn && selectedAction)
	//	{
	//		playerController.PerformCombatAction();
	//		selectedAction = false;
	//	}
	//}

	//private void OnMove(CallbackContext context)
	//{
	//	playerController.SetMoveDirection(context.ReadValue<Vector2>());
	//}
	//private void OnAttack(CallbackContext context)
	//{
	//	if (inCombat && activeTurn)
	//	{
	//		if (!selectedAction)
	//		{
	//			playerController.StartAttack();
	//			selectedAction = true;
	//		}
	//		else
	//		{
	//			EndTurn();
	//		}
	//	}
	//	else
	//	{
	//		if (context.started)
	//		{
	//			playerController.StartAttack();
	//		}
	//		if (context.canceled)
	//		{
	//			playerController.PerformAttack();
	//		}
	//	}
	//}
	//private void OnSpecial(CallbackContext context)
	//{
	//	if (context.started)
	//	{
	//		playerController.StartSpecialAttack();
	//	}
	//	if (context.canceled)
	//	{
	//		playerController.PerformSpecialAttack();
	//	}
	//}
	//private void OnThrow(CallbackContext context)
	//{
	//	if (context.started)
	//	{
	//		playerController.StartThrow();
	//	}
	//	else if (context.canceled)
	//	{
	//		playerController.PerformThrow();
	//	}
	//}
	//private void OnPickup(CallbackContext context)
	//{
	//	if (context.performed)
	//	{
	//		playerController.PerformPickup();
	//	}
	//}
}
