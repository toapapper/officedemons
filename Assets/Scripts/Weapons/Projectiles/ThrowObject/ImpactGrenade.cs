using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactGrenade : GrenadeObject
{
	private void OnCollisionEnter(Collision collision)
	{
		Explode();
	}
	private void FixedUpdate()
	{
		if (transform.position.y < 0.2f)
		{
			Explode();
		}
	}
}
