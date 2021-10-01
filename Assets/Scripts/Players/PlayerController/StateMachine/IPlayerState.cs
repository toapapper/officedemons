using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public interface IPlayerState
{
    bool IsActionTriggered { get; set; }
    bool IsActionLocked { get; set; }
    bool IsStaminaDepleted { get; }
    void LockAction();
    void CancelAction();
	void OnAttack();
    void OnSpecial();
    void OnPickUp(GameObject weapon);
	void OnThrow(CallbackContext context);
	//void OnPickupThrow(CallbackContext context);
    void OnRevive(CallbackContext context);
    void OnFixedUpdateState();
    void OnStateExit();
    void OnStateEnter();
}
