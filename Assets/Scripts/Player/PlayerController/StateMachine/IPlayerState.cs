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
    bool OnStartBombard();
    bool OnBombard();
    void OnSpecial();
    void OnPickUp(GameObject weapon);
    bool OnStartThrow();
	bool OnThrow();
    void OnRevive(GameObject player);
    void TransitionState(IPlayerState state);
    void OnFixedUpdateState();
    void OnStateExit();
    void OnStateEnter();
}
