using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactGrenade : GrenadeObject
{
	private void OnCollisionEnter(Collision collision)
	{
		//TODO if ground
		//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
		//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
		base.SetExplosion();
	}
}
