using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoffeeGrenade : MonoBehaviour
{
	protected GameObject thrower;
	protected bool isObjectThrown;

	private void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (transform.position.y < 0.2f)
			{
				Explode();
			}
			else if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				Explode();
			}
		}
		else
		{
			isObjectThrown = true;
		}
	}

	protected abstract void Explode();
}
