using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactGrenade : GrenadeObject
{
	private void OnCollisionEnter(Collision collision)
	{
		GetComponent<Rigidbody>().isKinematic = true;
		base.SetExplosion();
	}
}
