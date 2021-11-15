using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

/// <summary>
/// <para>
/// Spawns new player setup menu for every player joining in the menu
/// </para> 
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 12/11 -21
public class SpawnPlayerSetupMenu : MonoBehaviour
{
	public GameObject playerSetupMenuPrefab;
	public PlayerInput playerInput;

	private void Awake()
	{
		var rootMenu = GameObject.Find("Grid");
		Debug.Log(rootMenu);
		if(rootMenu != null)
		{
			var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
			playerInput.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
			menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(playerInput.playerIndex);
		}
	}
}
