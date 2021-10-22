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

// Last Edited: 22/10/2021
public class ProceduralTransformation : MonoBehaviour
{
    /// <summary>
    /// A list of what the item can turn into
    /// </summary>
    public List<GameObject> normalRoom = new List<GameObject>();
    public List<GameObject> Encounter = new List<GameObject>();
    public List<GameObject> Special = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        switch (SpawnItemsFromLibrary.currentScenary)
        {
            case Scenary.Normal:
                TransformItem(normalRoom);
                break;
            case Scenary.Encounter:
                TransformItem(Encounter);
                break;
            case Scenary.Special:
                TransformItem(Special);
                break;
            default:
                break;
        }
    }



    private void TransformItem(List<GameObject> list)
    {
        //Take a random item from the list and transform into it
        int rnd = Random.Range(0, list.Count -1);
        GameObject GO = Instantiate(list[rnd], this.transform.position, Quaternion.identity);
        GO.name = list[rnd].name;
        Destroy(gameObject);
    }

}
