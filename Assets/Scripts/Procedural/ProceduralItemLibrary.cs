using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>
/// Holds a Dictionary of items that can be spawned in, using a key value.
/// </para>
///  <para>
///  Author: Tim & Kristian
/// </para>
/// </summary>

// Last Edited: 13-10-2021
public class ProceduralItemLibrary : MonoBehaviour
{
    public static ProceduralItemLibrary Instance { get; private set; }

    public Dictionary<Vector2, GameObject> itemLibrary;
    [SerializeField]
    private List<GameObject> itemList = new List<GameObject>();


    public Dictionary<Vector2, GameObject> housesDictonary;


    [SerializeField]
    private List<GameObject> houses = new List<GameObject>();


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        itemLibrary = new Dictionary<Vector2, GameObject>();
        for (int i = 0; i < itemList.Count; i++)
        {
            itemLibrary.Add(new Vector2(itemList[i].GetComponent<BoxCollider>().size.x * itemList[i].transform.localScale.x, itemList[i].GetComponent<BoxCollider>().size.z * itemList[i].transform.localScale.z), itemList[i]);
        }

        housesDictonary = new Dictionary<Vector2, GameObject>();
        for (int i = 0; i < houses.Count; i++)
        {
            housesDictonary.Add(new Vector2(houses[i].GetComponent<BoxCollider>().size.x * houses[i].transform.localScale.x, houses[i].GetComponent<BoxCollider>().size.y * houses[i].transform.localScale.y), houses[i]);
        }

    }
    /// <summary>
    /// Gets an item from the dictinary with the right key value.
    /// </summary>
    /// <param name="key">The key value used to search the Dictionary</param>
    /// <param name="dictionary">ProceduralItemLibrary.Instance.itemLibrary/housesDictionary</param>
    /// <returns></returns>
    public GameObject GetItemFromDictionary(Vector2 key, Dictionary<Vector2,GameObject> dictionary)
    {
        GameObject item;

        if (dictionary.TryGetValue(key, out item))
        {
            return item;
        }
        else
        {
            Debug.LogError("Key did not match any items in library");
            return null;
        }        
    }
}
