using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
	public GameObject playerSetupMenuPrefab;

	private void Awake()
	{
		var rootMenu = GameObject.Find("MainLayout");
		if(rootMenu != null)
		{
			var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
		}
	}
}
