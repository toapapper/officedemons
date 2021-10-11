using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTransformation : MonoBehaviour
{
    public List<GameObject> transformations = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        int rnd = Random.Range(0, transformations.Count);
        GameObject GO = Instantiate(transformations[rnd], this.transform.position, Quaternion.identity);
        GO.name = transformations[rnd].name;
        Destroy(gameObject);
    }
}
