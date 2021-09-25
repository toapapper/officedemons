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
        switch (type)
        {
            case "Car":
                gameObject.GetComponent<MeshRenderer>().material = GetNameFromList(type);
                break;
            case "Bush":
                gameObject.GetComponent<MeshRenderer>().material = GetNameFromList(type);
                break;
            default:
                break;
        }
    }



    private string Evaluate(GameObject gameObject)
    {
        string mesh;
        int fitness = 0;
        Vector3 scale = gameObject.transform.localScale;
        if (scale.x * scale.y > 15)
            fitness += 100;

        if (scale.x < 5 || scale.z < 5)
        {
            fitness -= 10;
        }

        if (fitness < 10)
        {
            mesh = "Bush";
        }
        else
        {
            mesh = "Car";
        }
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
