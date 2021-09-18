using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isHeld;
	[SerializeField]
    private float damage = 10f;
	[SerializeField]
	private float throwDamage = 15f;

    public void Hit()
    {
        Debug.Log("Hit" + damage);
    }

    public void Throw(Vector3 force)
    {
        Debug.Log("Throw: " + throwDamage + ", " + force);
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
        isHeld = false;
    }
}
