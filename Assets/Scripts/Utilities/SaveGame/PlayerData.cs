using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public string playerName;
	public int playerHealth;
	//public int kills;
	//public int charges;
	public bool hasWeapon;

	public string weaponName;
	public int durability;
	

	public PlayerData(GameObject player)
	{
		playerName = player.name;
		playerHealth = player.GetComponent<Attributes>().Health;
		hasWeapon = player.GetComponent<WeaponHand>().objectInHand;
		//kills = player.GetComponent<Attributes>().Kills;
		//charges = player.GetComponent<SpecialHand>().objectInHand.Charges

		if (hasWeapon)
		{
			weaponName = player.GetComponent<WeaponHand>().objectInHand.name;
			durability = player.GetComponent<WeaponHand>().objectInHand.Durability;
		}
	}
}
