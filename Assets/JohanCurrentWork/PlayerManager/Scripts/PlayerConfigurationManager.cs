using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigurations;

    [SerializeField]
    private int maxPlayers = 1;

	[SerializeField]
	private string gameSceneName;

    public static PlayerConfigurationManager Instance { get; private set; }

	private void Awake()
	{
		if(Instance != null)
		{
			Debug.Log("SINGLETON - Trying to create another instace of singleton!!");
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(Instance);
			playerConfigurations = new List<PlayerConfiguration>();
		}
	}

	public List<PlayerConfiguration> GetPlayerConfigurations()
	{
		return playerConfigurations;
	}

	//Change to specific player prefab instead of color?????
	public void SetPlayerColor(int index, Material color)
	{
		playerConfigurations[index].PlayerMaterial = color;
	}
	public void ReadyPlayer(int index)
	{
		playerConfigurations[index].IsReady = true;
		if (playerConfigurations.Count >= maxPlayers && playerConfigurations.All(p => p.IsReady == true))
		{
			SceneManager.LoadScene(gameSceneName);
		}
	}
	public void HandlePlayerJoin(PlayerInput playerInput)
	{
		Debug.Log("Player joined " + playerInput.playerIndex);
		if(!playerConfigurations.Any(p => p.PlayerIndex == playerInput.playerIndex))
		{
			playerInput.transform.SetParent(transform);
			playerConfigurations.Add(new PlayerConfiguration(playerInput));
		}
	}
}

public class PlayerConfiguration
{
	public PlayerConfiguration(PlayerInput playerInput)
	{
		PlayerIndex = playerInput.playerIndex;
		Input = playerInput;
	}
    public PlayerInput Input { get; set; }
	public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }

    public Material PlayerMaterial { get; set; }
}
