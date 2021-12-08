using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    Transform checkPointPos;
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
        //GameManager.Instance.ResetEncounter();
        //
        camPos.transform.position = checkPointPos.position;
        //Debug.LogError("CAMERAMOVE");
        Camera.main.GetComponent<MultipleTargetCamera>().ObjectsInCamera = new List<GameObject>();

        int playerCounter = 0;
        foreach (GameObject player in PlayerManager.players)
        {
            //player.GetComponent<WeaponHand>().DropWeapon();

            PlayerData playerData = SaveSystem.LoadPlayer(player.name);

            Vector3 newPos = new Vector3(positions[playerCounter].position.x, player.transform.position.y, positions[playerCounter].position.z);
            Debug.Log("NEW POSITION:       " + newPos);

            player.transform.position = newPos;
            Debug.Log("PLAYER POSITION:       " + player.transform.position);

            //
            //Effects.Revive(player);
            //

            //player.GetComponent<PlayerMovementController>().MoveAmount = Vector3.zero;
            //player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //player.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            //player.GetComponent<NavMeshAgent>().isStopped = true;

            player.GetComponent<Attributes>().Health = playerData.playerHealth;
            player.GetComponent<Attributes>().KillCount = playerData.kills;
            player.GetComponent<SpecialHand>().ObjectInHand.Charges = playerData.charges;

            playerCounter++;
        }


        foreach (Transform weapon in GameObject.Find("Weapons").transform)
        {
            Debug.Log("WEAPON FOUND");
            //RemoveFromNearbyLists
            //foreach(GameObject player in PlayerManager.players)
            //{
            //	player.GetComponent<PlayerInputHandler>().RemoveObjectFromWeaponList(weapon.GetChild(0).gameObject);
            //}

            Destroy(weapon.gameObject);
            foreach (GameObject player in PlayerManager.players)
            {
                player.GetComponent<PlayerInputHandler>().ClearNearbyObjectList();
            }
        }

        List<WeaponData> weaponDataList = SaveSystem.LoadWeapons();
        foreach (WeaponData weaponData in weaponDataList)
        {
            //Debug.Log(weaponData.weaponType + "Handle");
            if (Resources.Load(weaponData.weaponType + "Handle"))
            {
                GameObject newWeapon = Instantiate(Resources.Load(weaponData.weaponType + "Handle"),
                    new Vector3(weaponData.position[0], weaponData.position[1], weaponData.position[2]),
                    Quaternion.Euler(0, 0, 0)) as GameObject;
                newWeapon.transform.parent = GameObject.Find("Weapons").transform;
                newWeapon.name = weaponData.weaponName;
                AbstractWeapon abstractWeapon = newWeapon.GetComponentInChildren<AbstractWeapon>();
                //abstractWeapon.gameObject.name = weaponData.weaponName;
                abstractWeapon.Damage = weaponData.damage;
                abstractWeapon.HitForce = weaponData.hitForce;
                abstractWeapon.ThrowDamage = weaponData.throwDamage;
                abstractWeapon.ViewDistance = weaponData.viewDistance;
                abstractWeapon.ViewAngle = weaponData.viewAngle;
                abstractWeapon.Durability = weaponData.durability;
                abstractWeapon.Weight = weaponData.weight;
                abstractWeapon.EffectList = weaponData.effects;
                Color outlineColor = new Color();
                for (int i = 0; i < 4; i++)
                {
                    outlineColor[i] = weaponData.outlineColor[i];
                }
                abstractWeapon.GetComponentInChildren<Outline>().OutlineColor = outlineColor;

                if (abstractWeapon is BurstShotWeapon)
                {
                    newWeapon.GetComponentInChildren<BurstShotWeapon>().BulletsInBurst = weaponData.bulletsInBurst;
                }

                if (abstractWeapon is RangedWeapon)
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

        }

        GameManager.Instance.ResetEncounter();
        isSaved = false;
        //Camera.main.GetComponent<MultipleTargetCamera>().ObjectsInCamera = PlayerManager.players;

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
        AkSoundEngine.SetState("Music_State", "Roaming");
        AkSoundEngine.SetState("Music", "RoamingState1");
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
