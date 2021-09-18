using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHand : MonoBehaviour
{
    private float damage = 5f;

    [SerializeField]
    private float throwForce = 10f;

    [SerializeField]
    private GameObject handObject;

    private GameObject objectInHand;

    public void Equip(GameObject newObject)
	{
        newObject.GetComponent<Weapon>().isHeld = true;
        newObject.GetComponent<Rigidbody>().useGravity = false;
        newObject.GetComponent<Rigidbody>().isKinematic = true;

        newObject.transform.parent = handObject.transform;
        newObject.transform.position = handObject.transform.position;
        newObject.transform.rotation = handObject.transform.rotation;
        
        objectInHand = newObject;

        //newObject.GetComponent<Weapon>().PickUpObject(handObject.transform);
    }

    public void DropObject()
    {
        objectInHand.transform.parent = null;
        objectInHand.GetComponent<Rigidbody>().useGravity = true;
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.GetComponent<Weapon>().isHeld = false;
        objectInHand = null;
    }

    public  void Hit()
    {
        if (objectInHand != null)
		{
            objectInHand.GetComponent<Weapon>().Hit();
        }
		else
		{
            Debug.Log("Hit" + damage);
        }
    }
    public void Throw(Vector3 direction)
	{
		if (objectInHand != null)
		{
            objectInHand.GetComponent<Weapon>().Throw(direction * throwForce);
            objectInHand = null;
        }
	}
}
