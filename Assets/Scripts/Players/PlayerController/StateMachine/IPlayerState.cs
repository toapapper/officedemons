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
    bool IsActionLocked { get; set; }
    void OnMove(CallbackContext context);
    void OnAttack();
    void OnSpecial();
    void OnPickupThrow(CallbackContext context);
    void OnRevive(CallbackContext context);
    void OnFixedUpdateState();
    void OnStateExit();
    void OnStateEnter();
}
