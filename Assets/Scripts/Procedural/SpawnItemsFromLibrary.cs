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

// Last Edited: 22-10-2021

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

    public GameObject level;
    /// <summary>
    /// Finds the closest key value from a Dictionary of values.
    /// takes the largest key which isn't bigger than nodes.x or nodes.y
    /// </summary>
    /// <param name="node"></param>
    public void FindClosestKey(Node node)
    {
        Vector2 bestFit = Vector2.zero;
        foreach (Vector2 key in ProceduralItemLibrary.Instance.itemLibrary.Keys)
        {
            if (key.x > node.size.x || key.y > node.size.y)
            {

            }
            else
            {
                if (key.x * key.y >= bestFit.x * bestFit.y)
                {
                    bestFit = key;
                }
            }
        }
        node.size = bestFit;
        item = ProceduralItemLibrary.Instance.GetItemFromDictionary(bestFit);
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
            item = ProceduralItemLibrary.Instance.GetItemFromDictionary(bestFit);
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
