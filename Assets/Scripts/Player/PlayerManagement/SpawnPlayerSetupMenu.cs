using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class SpawnPlayerSetupMenu : MonoBehaviour
{
	public GameObject playerSetupMenuPrefab;
	public PlayerInput playerInput;

	private void Awake()
	{
		var rootMenu = GameObject.Find("MainLayout");
		if(rootMenu != null)
		{
			var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
			playerInput.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
			menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(playerInput.playerIndex);
		}
	}
}
