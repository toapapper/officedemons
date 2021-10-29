using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCoffee : GrenadeObject
{
	private void OnCollisionEnter(Collision collision)
	{
		FOVVisualization.SetActive(true);
		base.Explode();
	}
}
