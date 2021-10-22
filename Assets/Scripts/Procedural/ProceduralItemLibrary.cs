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
    public List<GameObject> itemList = new List<GameObject>();
    
    private void Awake()
    {
        Instance = this;
        //itemLibrary = new Dictionary<Vector2, GameObject>();     
    }

    private void Start()
    {
        itemLibrary = new Dictionary<Vector2, GameObject>();
        for (int i = 0; i < itemList.Count; i++)
        {
            itemLibrary.Add(new Vector2(itemList[i].transform.localScale.x, itemList[i].transform.localScale.z), itemList[i]);
        }
    }
    /// <summary>
    /// Gets an item from the dictinary with the right key value.
    /// </summary>
    /// <param name="key">The key value used to search the Dictionary</param>
    /// <returns></returns>
    public GameObject GetItemFromDictionary(Vector2 key)
    {
        GameObject item;
        if(itemLibrary.TryGetValue(key, out item))
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
