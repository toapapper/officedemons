using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

/// <summary>
/// <para>
/// Script to handle saving and loading data about objects in game
/// </para><para>
/// Author: Jonas and Johan
/// </para>
/// </summary>
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






	public static void SaveDestructibles(List<GameObject> destructibleList)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		List<DestructibleData> destructibleDataList = new List<DestructibleData>();
		foreach (GameObject destructible in destructibleList)
		{
			DestructibleData destructibleData = new DestructibleData(destructible);
			destructibleDataList.Add(destructibleData);
		}

		string path = Application.persistentDataPath + "/DestructibleDataList.save";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, destructibleDataList);
		stream.Close();
	}


	public static List<DestructibleData> LoadDestructibles()
	{
		
		string path = Application.persistentDataPath + "/DestructibleDataList.save";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			List<DestructibleData> destructibleDataList = formatter.Deserialize(stream) as List<DestructibleData>;
			stream.Close();

			return destructibleDataList;
		}
		else
		{
			Debug.LogError("Save file not found" + path);
			return null;
		}
	}
}
