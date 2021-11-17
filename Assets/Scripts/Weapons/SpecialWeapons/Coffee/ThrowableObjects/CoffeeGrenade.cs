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

// Last Edited: 15-11-17
public abstract class CoffeeGrenade : MonoBehaviour
{
	protected GameObject thrower;
	protected bool isObjectThrown;
	[SerializeField]
	private GameObject particleEffect;

	protected int maxDistance = 30;

	private void FixedUpdate()
	{
		if (transform.position.y < -10f)
		{
			GameManager.Instance.StillCheckList.Remove(gameObject);
			Destroy(gameObject);
		}
	}

	protected abstract void Explode();

	protected abstract void CreateStain(Vector3 stainPosition);

	private void OnCollisionEnter(Collision collision)
	{
		Instantiate(particleEffect, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
		Explode();
		if (collision.gameObject.transform.tag == "Ground")
		{
			CreateStain(collision.contacts[0].point);
		}
		else
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, LayerMask.GetMask("Ground")))
			{
				CreateStain(hit.point);
			}
		}
		GameManager.Instance.StillCheckList.Remove(gameObject);
		Destroy(gameObject);
	}
}
