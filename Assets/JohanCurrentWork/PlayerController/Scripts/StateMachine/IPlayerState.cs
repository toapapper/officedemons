using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public interface IPlayerState
{
    void OnMove(CallbackContext context);
    void OnAttack(CallbackContext context);
    void OnSpecial(CallbackContext context);
    void OnThrow(CallbackContext context);
    void OnPickup(CallbackContext context);
    void OnRevive(CallbackContext context);
    void OnFixedUpdateState();
    void OnStateExit();
    void OnStateEnter();
}
