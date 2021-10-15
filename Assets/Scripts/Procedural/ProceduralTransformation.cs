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

// Last Edited: 13/10/2021
public class ProceduralTransformation : MonoBehaviour
{
    /// <summary>
    /// A list of what the item can turn into
    /// </summary>
    public List<GameObject> transformations = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Take a random item from the list and transform into it
        int rnd = Random.Range(0, transformations.Count);
        GameObject GO = Instantiate(transformations[rnd], this.transform.position, Quaternion.identity);
        GO.name = transformations[rnd].name;
        Destroy(gameObject);
    }
}
