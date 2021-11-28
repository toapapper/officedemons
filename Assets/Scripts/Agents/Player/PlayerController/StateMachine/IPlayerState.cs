using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// <para>
/// Interface to player states
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 2021-10-12
public interface IPlayerState
{
    TypeOfAction ChosenAction { get; set; }
    bool IsAddingBombardForce { get; set; }
    bool IsActionTriggered { get; set; }
    bool IsActionLocked { get; set; }
    bool IsStaminaDepleted { get; }
    void LockAction();
    void CancelAction();
	void OnAttack();
    bool OnStartBombard();
    bool OnBombard();
    void OnSpecial();
    bool OnStartSpecialBombard();
    bool OnSpecialBombard();
    void OnPickUp(GameObject weapon);
    bool OnStartThrow();
	bool OnThrow();
    void OnRevive(GameObject player);
    void TransitionState(IPlayerState state);
    void OnFixedUpdateState();
    void OnStateExit();
    void OnStateEnter();
}
