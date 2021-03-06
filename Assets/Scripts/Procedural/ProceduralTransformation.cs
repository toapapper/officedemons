using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Put this on an item in the item library to make it transform into various different objects
/// <para>
/// Author: Tim
/// </para>
/// </summary>
/// 

// Last Edited: 23/10/2021
public class ProceduralTransformation : MonoBehaviour
{
    /// <summary>
    /// A list of what the item can turn into
    /// </summary>
    public List<GameObject> normalRoomCity = new List<GameObject>();
    public List<GameObject> normalRoomForest = new List<GameObject>();
    public List<GameObject> normalRoomRural = new List<GameObject>();

    public List<GameObject> EncounterCity = new List<GameObject>();
    public List<GameObject> EncounterForest = new List<GameObject>();
    public List<GameObject> EncounterRural = new List<GameObject>();

    public List<GameObject> SpecialCity = new List<GameObject>();
    public List<GameObject> SpecialForest = new List<GameObject>();
    public List<GameObject> SpecialRural = new List<GameObject>();

    [SerializeField]
    private bool SkipScenary;
    [SerializeField]
    private List<GameObject> misc = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        if (!SkipScenary)
        {
            switch (FitnessFunction.currentRoom)
            {
                case Rooms.Normal:
                    switch (SpawnItemsFromLibrary.currentScenary)
                    {
                        case Scenary.City:
                            TransformItem(normalRoomCity);
                            break;
                        case Scenary.Forest:
                            TransformItem(normalRoomForest);
                            break;
                        case Scenary.Rural:
                            TransformItem(normalRoomRural);
                            break;
                        default:
                            break;
                    }
                    break;
                case Rooms.Encounter:
                    switch (SpawnItemsFromLibrary.currentScenary)
                    {
                        case Scenary.City:
                            TransformItem(EncounterCity);
                            break;
                        case Scenary.Forest:
                            TransformItem(EncounterForest);
                            break;
                        case Scenary.Rural:
                            TransformItem(EncounterRural);
                            break;
                        default:
                            break;
                    }
                    break;
                case Rooms.Special:
                    switch (SpawnItemsFromLibrary.currentScenary)
                    {
                        case Scenary.City:
                            TransformItem(SpecialCity);
                            break;
                        case Scenary.Forest:
                            TransformItem(SpecialForest);
                            break;
                        case Scenary.Rural:
                            TransformItem(SpecialRural);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            TransformItem(misc);
        }

    }



    private void TransformItem(List<GameObject> list)
    {
        //Take a random item from the list and transform into it
        int rnd = Random.Range(0, list.Count);
        GameObject GO = Instantiate(list[rnd], new Vector3(this.transform.position.x, list[rnd].gameObject.transform.position.y, this.transform.position.z), list[rnd].transform.rotation,transform.parent);
        GO.name = list[rnd].name;
        if (GO.CompareTag("Enemy"))
        {
            GO.transform.parent = FindClosestEncounter();
        }
        Destroy(gameObject);
    }



    private Transform FindClosestEncounter()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Encounter");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest.transform;
    }

}
