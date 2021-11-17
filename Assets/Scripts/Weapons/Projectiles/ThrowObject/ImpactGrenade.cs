using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactGrenade : GrenadeObject
{
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("EEEEEXPLOOOOOOOOOOOOSSSIIIIIIIIIIIOOOOOOOOOOOOOOOOOONNNNNNNNNNNNNNNNNNNNNNNN from hitting something. a grenade said this.");

		Explode();
	}
	private void FixedUpdate()
	{
		if (transform.position.y < -10f)
		{
			Explode();
		}
	}
}
