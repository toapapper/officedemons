using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Instatiate all chosen players in level
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 18/10 -21
public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpwns;
    [SerializeField]
    private GameObject[] playerPrefabs;


    void Awake()
    {
        PlayerConfiguration[] playerConfigurations = PlayerConfigurationManager.Instance.GetPlayerConfigurations().ToArray();

		for (int i = 0; i < playerConfigurations.Length ; i++)
		{
            GameObject player = Instantiate(playerPrefabs[playerConfigurations[i].CharacterIndex], playerSpwns[i].position, playerSpwns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigurations[i]);
		}
    }
}
