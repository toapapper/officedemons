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
    public enum Scenary { Town,City,Forest}

public class SpawnItemsFromLibrary : MonoBehaviour
{
    GameObject item;

    //Use it in transformation
    public static Scenary currentScenary = Scenary.Forest;

    public GameObject level;
    /// <summary>
    /// Finds the closest key value from a Dictionary of values.
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
    /// <summary>
    /// Spawns the selected item on the nodes position.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="root"></param>
    public void SpawnItems(Node node, Node root)
    {
        GameObject GO = Instantiate(item, new Vector3(node.position.x, item.transform.lossyScale.y /2 , node.position.y), Quaternion.identity);
        GO.transform.parent = level.transform;
        GO.name = item.name;
        node.gameObject = GO;
    }
}
