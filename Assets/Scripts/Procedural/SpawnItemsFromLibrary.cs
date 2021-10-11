using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemsFromLibrary : MonoBehaviour
{
    GameObject item;
    public GameObject level;

    public void FindClosestKey(Node node)
    {
        Vector2 bestfit = Vector2.zero;
        foreach (Vector2 key in ProceduralItemLibrary.Instance.itemLibrary.Keys)
        {
            if (key.x > node.size.x || key.y > node.size.y)
            {

            }
            else
            {
                if (key.x * key.y >= bestfit.x * bestfit.y)
                {
                    bestfit = key;
                }
            }
        }
        node.size = bestfit;
        item = ProceduralItemLibrary.Instance.GetItemFromDictionary(bestfit);
    }

    public void SpawnItems(Node node, Node root)
    {
        GameObject GO = Instantiate(item, new Vector3(node.position.x, item.transform.lossyScale.y /2 , node.position.y), Quaternion.identity);
        GO.transform.parent = level.transform;
        GO.name = item.name;
        node.gameObject = GO;
    }
}
