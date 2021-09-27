using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void OnFixedUpdateState();
    void OnStateExit();
    void OnStateEnter();
}
