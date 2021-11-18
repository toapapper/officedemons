using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityGrenade : GrenadeObject
{
	private void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				base.SetExplosion();
			}
			else if(transform.position.y < 0f)
			{
				//TEST
				GetComponent<Rigidbody>().isKinematic = true;
				base.SetExplosion();
			}
		}
		else
		{
			isObjectThrown = true;
		}
	}
}
