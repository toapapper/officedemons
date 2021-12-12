using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DestructibleData
{
    public float[] position;
    public float[] rotation;
    public string destructibleName;

    public bool destroyd;
    public float objectHealth;

    public DestructibleData(GameObject destructibleObject)
    {
        position = new float[3];
        position[0] = destructibleObject.transform.position.x;
        position[1] = destructibleObject.transform.position.y;
        position[2] = destructibleObject.transform.position.z;

        rotation = new float[3];
        rotation[0] = destructibleObject.transform.rotation.eulerAngles.x;
        rotation[1] = destructibleObject.transform.rotation.eulerAngles.y;
        rotation[2] = destructibleObject.transform.rotation.eulerAngles.z;

        destructibleName = destructibleObject.name;
        Debug.Log("DESTRUCTIBLE NAME:      " + destructibleObject.name);
        if (destructibleName.Contains("("))
        {
            destructibleName = destructibleName.Remove(destructibleObject.name.IndexOf('('));
            destructibleName.TrimEnd(' ');
        }

        if (destructibleObject.GetComponent<DestructibleObjects>())
		{
            objectHealth = destructibleObject.GetComponent<Attributes>().Health;
            destroyd = destructibleObject.GetComponent<DestructibleObjects>().destroyed;
        }
    }
}
