using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public string playerName;
	public int playerHealth;
	public int kills;
	public int charges;
	public bool hasWeapon;

	public string weaponName;
	public int durability;
	

	public PlayerData(GameObject player)
	{
		playerName = player.name;
		playerHealth = player.GetComponent<Attributes>().Health;
		kills = player.GetComponent<Attributes>().KillCount;
		charges = player.GetComponent<SpecialHand>().ObjectInHand.Charges;
		hasWeapon = player.GetComponent<WeaponHand>().objectInHand;

		if (hasWeapon)
		{
			weaponName = player.GetComponent<WeaponHand>().objectInHand.name;
			durability = player.GetComponent<WeaponHand>().objectInHand.Durability;
		}
	}
}
