using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    public static void SavePlayer(GameObject player)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		PlayerData playerData = new PlayerData(player);

		string path = Application.persistentDataPath + "/" + playerData.playerName + ".save";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, playerData);
		stream.Close();
	}
	public static PlayerData LoadPlayer(string playerName)
	{
		string path = Application.persistentDataPath + "/" + playerName + ".save";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			PlayerData playerdata = formatter.Deserialize(stream) as PlayerData;
			stream.Close();

			return playerdata;
		}
		else
		{
			Debug.LogError("Save file not found" + path);
			return null;
		}
	}

	public static void SaveWeapons(List<GameObject> weaponList)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		List<WeaponData> weaponDataList = new List<WeaponData>();
		foreach (GameObject weapon in weaponList)
		{
			WeaponData weaponData = new WeaponData(weapon);
			weaponDataList.Add(weaponData);
		}
		string path = Application.persistentDataPath + "/WeaponDataList.save";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, weaponDataList);
		stream.Close();
	}

	public static List<WeaponData> LoadWeapons()
	{
		string path = Application.persistentDataPath + "/WeaponDataList.save";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			List<WeaponData> weaponDataList = formatter.Deserialize(stream) as List<WeaponData>;
			stream.Close();

			return weaponDataList;
		}
		else
		{
			Debug.LogError("Save file not found" + path);
			return null;
		}
	}
}
