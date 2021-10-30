using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGrenade : GrenadeObject
{
	private void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				base.SetExplosion();
			}
		}
		else
		{
			isObjectThrown = true;
		}
	}
}
