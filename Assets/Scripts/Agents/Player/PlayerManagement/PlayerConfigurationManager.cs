using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>
/// Handles playerconfiguration and saves selections made in selection menu
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 18/10 -21
public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigurations;

    [SerializeField]
    private int minimumPlayers = 1;

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
	public void SetPlayerCharacter(int playerIndex, int characterIndex)
	{
		playerConfigurations[playerIndex].CharacterIndex = characterIndex;
	}
	public void ReadyPlayer(int index)
	{
		playerConfigurations[index].IsReady = true;
		if (playerConfigurations.Count >= minimumPlayers && playerConfigurations.All(p => p.IsReady == true))
		{
			SceneManager.LoadScene(gameSceneName);
		}
	}
	public void HandlePlayerJoin(PlayerInput playerInput)
	{
		Debug.Log("Player joined " + playerInput.playerIndex);
		PlayerConfiguration newPlayerConfig = null;

		if(playerConfigurations.Count < playerInput.playerIndex + 1)
		{
			Debug.Log("Creating new playerConfiguration");
			newPlayerConfig = new PlayerConfiguration(playerInput);
			playerInput.transform.SetParent(transform);
			playerConfigurations.Add(newPlayerConfig);
		}

		if(SceneManager.GetActiveScene().name != "PlayerSelection" && newPlayerConfig != null)
        {
			if(PlayerManager.Instance.JoinAnyTime)
			{
				Debug.Log("Spawning new player");
				PlayerManager.Instance.SpawnNewPlayer(newPlayerConfig);
			}
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
	public int CharacterIndex { get; set; }
}
