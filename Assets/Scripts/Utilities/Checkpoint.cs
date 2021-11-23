using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	[SerializeField]
	List<Transform> positions;
	GameObject camPos;

	private bool isSaved;

	//public Dictionary<Vector3, string> savedPlayers;

	public List<string> savedPlayers;


	public void Awake()
	{
		camPos = Camera.main.transform.parent.gameObject;
		//savedPlayers = new Dictionary<Vector3, string>();
		savedPlayers = new List<string>();
	}
	public void SaveCheckpoint()
	{
		GameManager.Instance.CurrentCheckpoint = this;

		List<GameObject> weaponList = new List<GameObject>();
		foreach (Transform weapon in GameObject.Find("Weapons").transform)
		{
			if (weapon.GetComponentInChildren<AbstractWeapon>())
			{
				Debug.Log("SAVEWEAPON");
				weaponList.Add(weapon.gameObject);
			}
		}
		foreach (GameObject player in PlayerManager.players)
		{
			SaveSystem.SavePlayer(player);

			if (player.GetComponent<WeaponHand>().objectInHand)
			{
				Debug.Log("SAVEPLAYERWEAPON");
				weaponList.Add(player.GetComponent<WeaponHand>().objectInHand.transform.parent.gameObject);
			}
		}
		SaveSystem.SaveWeapons(weaponList);
	}

	public void LoadCheckpoint()
	{
		//
		GameManager.Instance.ResetEncounter();
		//

		int playerCounter = 0;
		foreach (GameObject player in PlayerManager.players)
		{
			player.GetComponent<WeaponHand>().DropWeapon();

			PlayerData playerData = SaveSystem.LoadPlayer(player.name);

			//
			Effects.Revive(player);
			//

			player.GetComponent<Attributes>().Health = playerData.playerHealth;
			//if (playerData.hasWeapon)
			//{
			//	//Create weapon with playerData.weaponName and place in weaponHand
			//	//player.GetComponent<WeaponHand>().objectInHand.Durability = playerData.durability
			//}


			Vector3 newPos = new Vector3(positions[playerCounter].position.x, player.transform.position.y, positions[playerCounter].position.z);
			Debug.Log("NEW POSITION:       " + newPos);
			camPos.transform.position = transform.position;
			player.transform.position = newPos;
			Debug.Log("PLAYER POSITION:       " + player.transform.position);
			playerCounter++;
		}
		

		foreach (Transform weapon in GameObject.Find("Weapons").transform)
		{
			Debug.Log("WEAPON FOUND");
			Destroy(weapon.gameObject);
		}

		List<WeaponData> weaponDataList = SaveSystem.LoadWeapons();
		foreach (WeaponData weaponData in weaponDataList)
		{
			Debug.Log(weaponData.weaponType + "Handle");
			GameObject newWeapon = Instantiate(Resources.Load(weaponData.weaponType + "Handle"),
				new Vector3(weaponData.position[0], weaponData.position[1], weaponData.position[2]),
				Quaternion.Euler(0, 0, 0)) as GameObject;
			newWeapon.transform.parent = GameObject.Find("Weapons").transform;

			AbstractWeapon abstractWeapon = newWeapon.GetComponentInChildren<AbstractWeapon>();
			abstractWeapon.gameObject.name = weaponData.weaponName;
			abstractWeapon.Damage = weaponData.damage;
			abstractWeapon.HitForce = weaponData.hitForce;
			abstractWeapon.ThrowDamage = weaponData.throwDamage;
			abstractWeapon.ViewDistance = weaponData.viewDistance;
			abstractWeapon.ViewAngle = weaponData.viewAngle;
			abstractWeapon.Durability = weaponData.durability;
			abstractWeapon.Weight = weaponData.weight;
			abstractWeapon.EffectList = weaponData.effects;
			if(abstractWeapon is RangedWeapon)
			{
				newWeapon.GetComponentInChildren<RangedWeapon>().Inaccuracy = weaponData.inaccuracy;
			}

			if (!string.IsNullOrEmpty(weaponData.wielder))
			{
				Debug.Log(weaponData.wielder);
				Debug.Log(newWeapon);
				GameObject.Find(weaponData.wielder).GetComponent<WeaponHand>().Equip(abstractWeapon.gameObject);
			}
		}

		//GameManager.Instance.ResetEncounter();


		//int playerCounter = 0;
		//foreach (GameObject player in PlayerManager.players)
		//{
		//    Vector3 newPos = new Vector3(positions[playerCounter].position.x, player.transform.position.y, positions[playerCounter].position.z);
		//    player.transform.position = newPos;
		//    playerCounter++;
		//}
		//int i = 0;

		//      foreach(string player in savedPlayers)
		//{
		//          GameObject newPlayer = Instantiate(Resources.Load(player), positions[i].position, positions[i].rotation) as GameObject;
		//          newPlayer.transform.parent = PlayerManager.Instance.transform;
		//          i++;
		//      }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!isSaved)
			{
				SaveCheckpoint();
				isSaved = true;
			}
		}
	}
}
