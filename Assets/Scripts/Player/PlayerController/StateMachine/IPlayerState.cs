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
    void OnStartBombard();
    void OnBombard();
    void OnSpecial();
    void OnPickUp(GameObject weapon);
    void OnStartThrow();
	void OnThrow();
    void OnRevive(GameObject player);
    void TransitionState(IPlayerState state);
    void OnFixedUpdateState();
    void OnStateExit();
    void OnStateEnter();
}
