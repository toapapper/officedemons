using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The abstract grenade thrown by Devins special weapon
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-16
public abstract class CoffeeGrenade : MonoBehaviour
{
	protected GameObject thrower;
	protected bool isObjectThrown;
	[SerializeField]
	private GameObject particleEffect;

	private void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (transform.position.y < 0.2f)
			{
				Explode();
				Instantiate(particleEffect, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
			}
			else if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				Explode();
				Instantiate(particleEffect, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
			}
		}
		else
		{
			isObjectThrown = true;
		}
	}

	protected abstract void Explode();
}
