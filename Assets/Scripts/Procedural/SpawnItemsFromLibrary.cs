using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>
/// Instansiates items based on their bestfit key value.
/// </para>
///  <para>
///  Author: Tim & Kristian
/// </para>
/// </summary>

// Last Edited: 3-11-2021

    //The scenaries will change later just needed something to start with
    public enum Scenary { City, Forest, Rural}

public class SpawnItemsFromLibrary : MonoBehaviour
{
    private GameObject item;
    public GameObject Item
    {
        get { return item; }
    }
    //Use it in transformation
    public static Scenary currentScenary = Scenary.City;


    public static SpawnItemsFromLibrary Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public GameObject level;
    /// <summary>
    /// Finds the closest key value from a Dictionary of values.
    /// takes the largest key which isn't bigger than nodes.x or nodes.y
    /// </summary>
    /// <param name="node"></param>
    public GameObject FindClosestKey(Vector2 nodeSize, Dictionary<Vector2,GameObject> dictionary)
    {
        Vector2 bestFit = Vector2.zero;
        float closest = float.MaxValue;
        foreach (Vector2 key in dictionary.Keys)
        {
            if (dictionary == ProceduralItemLibrary.Instance.housesDictonary)
            {
                if (Mathf.Abs(key.y - nodeSize.y) < closest)
                {
                    closest = Mathf.Abs(key.y - nodeSize.y);
                    bestFit = key;
                }
            }
            else
            {
                if (key.x > nodeSize.x || key.y > nodeSize.y)
                {
                    //Too big cannot use
                }
                else
                {
                    if (key.x * key.y >= bestFit.x * bestFit.y)
                    {
                        bestFit = key;
                    }
                }

                nodeSize = bestFit;
            }
        }
        item = ProceduralItemLibrary.Instance.GetItemFromDictionary(bestFit, dictionary);
        return item;
    }

    public GameObject SeeClosestKey(Node node)
    {
        Vector2 bestFit = Vector2.zero;
        foreach (Vector2 key in ProceduralItemLibrary.Instance.itemLibrary.Keys)
        {
            if (key.x > node.size.x || key.y > node.size.y)
            {

            }
            else
            {
                if (key.x * key.y > bestFit.x * bestFit.y)
                {
                    bestFit = key;
                }
            }
        }
        node.size = bestFit;
        if (bestFit != Vector2.zero)
        {
            item = ProceduralItemLibrary.Instance.GetItemFromDictionary(bestFit, ProceduralItemLibrary.Instance.itemLibrary);
        }
        return item;
    }
    /// <summary>
    /// Spawns the selected item on the nodes position.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="root"></param>
    public void SpawnItems(Node node, Node root)
    {
        if (item != null)
        {
            GameObject GO = Instantiate(item, new Vector3(node.position.x, item.transform.lossyScale.y /2 , node.position.y), Quaternion.identity);
            GO.transform.parent = level.transform;
            GO.name = item.name;
            node.gameObject = GO;
            item = null;
        }
    }
}
