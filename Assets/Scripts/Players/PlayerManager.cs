using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    public static List<PlayerController> players;

    void Update()
    {
        if(players != null)
		{
            Debug.Log(players.Count);
        }
    }
}
