using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpwns;
    [SerializeField]
    private GameObject playerPrefabs;

    void Start()
    {
        PlayerConfiguration[] playerConfigurations = PlayerConfigurationManager.Instance.GetPlayerConfigurations().ToArray();

		for (int i = 0; i < playerConfigurations.Length ; i++)
		{
            GameObject player = Instantiate(playerPrefabs, playerSpwns[i].position, playerSpwns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigurations[i]);
		}
    }

}
