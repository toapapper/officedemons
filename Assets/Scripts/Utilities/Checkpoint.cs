using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform checkPointPos;
    [SerializeField] private List<Transform> positions;

    private GameObject camPos;

    private bool isSaved;
    //public List<string> savedPlayers;


    public void Awake()
    {
        camPos = Camera.main.transform.parent.gameObject;
        //savedPlayers = new List<string>();
    }
    public void SaveCheckpoint()
    {
        GameManager.Instance.CurrentCheckpoint = this;

        List<GameObject> weaponList = new List<GameObject>();
        foreach (Transform weapon in GameObject.Find("Weapons").transform)
        {
            if (weapon.GetComponentInChildren<AbstractWeapon>())
            {
                weaponList.Add(weapon.gameObject);
            }
        }
        foreach (GameObject player in PlayerManager.players)
        {
            SaveSystem.SavePlayer(player);

            if (player.GetComponent<WeaponHand>().objectInHand)
            {
                weaponList.Add(player.GetComponent<WeaponHand>().objectInHand.transform.parent.gameObject);
            }
        }
        SaveSystem.SaveWeapons(weaponList);

        //Destructible objects
        List<GameObject> destructibleList = new List<GameObject>();
        foreach (Transform destructible in GameObject.Find("DestructibleObjects").transform)
        {
            destructibleList.Add(destructible.gameObject);
        }
        SaveSystem.SaveDestructibles(destructibleList);
    }

    public void LoadCheckpoint()
    {
        //Camera
        camPos.transform.position = checkPointPos.position;
        Camera.main.GetComponent<MultipleTargetCamera>().ObjectsInCamera = new List<GameObject>();

        //Player
        int playerCounter = 0;
        foreach (GameObject player in PlayerManager.players)
        {
            Vector3 newPos = new Vector3(positions[playerCounter].position.x, player.transform.position.y, positions[playerCounter].position.z);
            player.transform.position = newPos;

            PlayerData playerData = SaveSystem.LoadPlayer(player.name);
            player.GetComponent<Attributes>().Health = playerData.playerHealth;
            player.GetComponent<Attributes>().KillCount = playerData.kills;
            player.GetComponent<SpecialHand>().ObjectInHand.Charges = playerData.charges;

            playerCounter++;
        }

        //Weapons
        foreach (Transform weapon in GameObject.Find("Weapons").transform)
        {
            Destroy(weapon.gameObject);
        }
        foreach (GameObject player in PlayerManager.players)
        {
            player.GetComponent<PlayerInputHandler>().ClearNearbyObjectList();
        }

        List<WeaponData> weaponDataList = SaveSystem.LoadWeapons();
        foreach (WeaponData weaponData in weaponDataList)
        {
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

                //does not remember the current uses left on the statuseffect when saved. But that should only possibly be a small balance-problem
                abstractWeapon.AddStatusEffects(weaponData.effects);

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
                    GameObject.Find(weaponData.wielder).GetComponent<WeaponHand>().Equip(abstractWeapon.gameObject);
                }
            }
        }

        //Destructible objects
        foreach (Transform destructible in GameObject.Find("DestructibleObjects").transform)
        {
            Destroy(destructible.gameObject);
        }

        List<DestructibleData> destructibleDataList = SaveSystem.LoadDestructibles();
        foreach (DestructibleData destructibleData in destructibleDataList)
        {
            if (Resources.Load(destructibleData.destructibleName))
            {
                GameObject newDestructible = Instantiate(Resources.Load(destructibleData.destructibleName),
                    new Vector3(destructibleData.position[0], destructibleData.position[1], destructibleData.position[2]),
                    Quaternion.Euler(destructibleData.rotation[0], destructibleData.rotation[1], destructibleData.rotation[2])) as GameObject;

                if (destructibleData.destroyd)
				{
                    foreach (MeshRenderer mr in newDestructible.GetComponentsInChildren<MeshRenderer>())
                    {
                        foreach (Material m in mr.materials)
                        {
                            m.color = Color.black;
                        }
                    }
                    newDestructible.GetComponent<DestructibleObjects>().destroyed = true;
                    newDestructible.GetComponent<Attributes>().SaveLoadHealth = 0;
                }
				else if(newDestructible.GetComponent<Attributes>())
				{
                    newDestructible.GetComponent<Attributes>().SaveLoadHealth = destructibleData.objectHealth;
                }

                newDestructible.transform.parent = GameObject.Find("DestructibleObjects").transform;
                newDestructible.name = destructibleData.destructibleName;
            }
        }

        GameManager.Instance.ResetEncounter();
        foreach (Transform skeleton in GameObject.Find("Skeletons").transform)
        {
            Destroy(skeleton.gameObject);
        }
        isSaved = false;

        AkSoundEngine.SetState("Music_State", "Roaming");
        AkSoundEngine.SetState("Music", "RoamingState1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!isSaved)
            {
                isSaved = true;
                SaveCheckpoint();
            }
        }
    }
}
