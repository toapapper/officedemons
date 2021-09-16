using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMesh : MonoBehaviour
{
    public List<Material> materials;
    double valueCorrector = 0.001;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GetTexture(GameObject gameObject)
    {
        string type = Evaluate(gameObject);
        Debug.Log(type);
        switch (type)
        {
            case "Car":
                gameObject.GetComponent<MeshRenderer>().material = GetNameFromList("Car");
                break;
            case "Cube":
                gameObject.GetComponent<MeshRenderer>().material = GetNameFromList("CarScuffed");
                break;
            default:
                break;
        }
    }



    private string Evaluate(GameObject gameObject)
    {
        string mesh;
        Vector3 scale = gameObject.transform.localScale;

        if (scale.x * scale.z > 20 * valueCorrector)
            mesh = "CarScuffed";
        else
            mesh = "Car";

        return mesh;
    }


    private Material GetNameFromList(string textureName)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i].name == textureName)
            {
                return materials[i];
            }
        }
        return materials[0];
    }
}
