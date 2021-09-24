using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by Johan
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfiguration;
    private Mover mover;

    [SerializeField]
    private MeshRenderer playerMesh;

    private InputActions inputControls;

	private void Awake()
	{
        mover = GetComponent<Mover>();
        inputControls = new InputActions();
	}

    public void InitializePlayer(PlayerConfiguration pc)
	{
        playerConfiguration = pc;
        //playerMesh.material = pc.PlayerMaterial;
		playerConfiguration.Input.onActionTriggered += Input_onActionTriggered;
    }

	private void Input_onActionTriggered(CallbackContext obj)
	{
		if(obj.action.name == inputControls.PlayerMovement.Move.name)
		{
			OnMove(obj);
		}
		else if(obj.action.name == inputControls.PlayerMovement.Attack.name)
		{
			OnAttack(obj);
		}
		else if (obj.action.name == inputControls.PlayerMovement.Special.name)
		{
			OnSpecial(obj);
		}
		else if (obj.action.name == inputControls.PlayerMovement.Throw.name)
		{
			OnThrow(obj);
		}
		else if (obj.action.name == inputControls.PlayerMovement.PickUp.name)
		{
			OnPickup(obj);
		}
	}
	private void OnMove(CallbackContext context)
	{
		if(mover != null)
		{
			mover.SetInputVector(context.ReadValue<Vector2>());
		}
	}
	private void OnAttack(CallbackContext context)
	{
		if (mover != null)
		{
			mover.SetAttack(context.ReadValue<float>());
		}
	}
	private void OnSpecial(CallbackContext context)
	{
		if (mover != null)
		{
			mover.SetSpecial(context.ReadValue<float>());
		}
	}
	private void OnThrow(CallbackContext context)
	{
		if (mover != null)
		{
			mover.SetThrow(context.ReadValue<float>());
		}
	}
	private void OnPickup(CallbackContext context)
	{
		if (mover != null)
		{
			mover.SetPickup(context.ReadValue<float>());
		}
	}
}
