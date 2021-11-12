using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    List<Transform> positions;

    //public Dictionary<Vector3, string> savedPlayers;

    public List<string> savedPlayers;


	public void Awake()
	{
        //savedPlayers = new Dictionary<Vector3, string>();
        savedPlayers = new List<string>();
    }
	public void SaveCheckpoint()
    {
        GameManager.Instance.CurrentCheckpoint = this;

        //int i = 0;
        foreach (GameObject player in PlayerManager.players)
        {
            SaveSystem.SavePlayer(player);





            //savedPlayers.Add(positions[i].position, player.name);
            //savedPlayers.Add(player.name);

            //PrefabUtility.SaveAsPrefabAsset(player, "Assets/Prefabs/Player/CharacterPrefabs/Resources/" + player.name + ".prefab");
            //i++;
        }
    }

    public void LoadCheckpoint()
    {
		int playerCounter = 0;
		foreach (GameObject player in PlayerManager.players)
		{
            PlayerData playerData = SaveSystem.LoadPlayer(player.name);
            player.GetComponent<Attributes>().Health = playerData.playerHealth;
			if (playerData.hasWeapon)
			{
                //Create weapon with playerData.weaponName and place in weaponHand
                //player.GetComponent<WeaponHand>().objectInHand.Durability = playerData.durability
            }
			Vector3 newPos = new Vector3(positions[playerCounter].position.x, player.transform.position.y, positions[playerCounter].position.z);
			player.transform.position = newPos;
			playerCounter++;
		}


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
            SaveCheckpoint();
        }
    }
}
