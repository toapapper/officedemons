using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCoffee : GrenadeObject
{
	private void OnCollisionEnter(Collision collision)
	{
		base.SetExplosion();
		//FOVVisualization.SetActive(true);
		//base.CountdownTime(explodeTime);
		//base.Explode();
	}
}
