using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The grenade object/projectile
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15/10-21
public class GrenadeObject : MonoBehaviour
{
	private GrenadeObject grenadeObject;
	[SerializeField]
	private GameObject FOVVisualization;
	private float grenadeDamage;
	private float grenadeExplodeForce;
	[SerializeField]
	private float initialExplodeTime = 2f;
	private float explodeTime;
	private bool isObjectThrown;

	public void CreateGrenade(Vector3 position, Vector3 direction, float grenadeThrowForce, float grenadeExplodeForce, float grenadeDamage)
	{
		grenadeObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		grenadeObject.grenadeDamage = grenadeDamage;
		grenadeObject.grenadeExplodeForce = grenadeExplodeForce;
		grenadeObject.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
		grenadeObject.explodeTime = initialExplodeTime;
		GameManager.Instance.stillCheckList.Add(grenadeObject.gameObject);
	}

	public void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				FOVVisualization.SetActive(true);
				StartCoroutine(CountdownTime(explodeTime));
			}
		}
		else
		{
			isObjectThrown = true;
		}
	}

	private IEnumerator CountdownTime(float time)
	{
		yield return new WaitForSeconds(time);
		Explode();
	}

	private void Explode()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().visibleTargets;

		foreach (GameObject target in targetList)
		{
			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			Effects.Damage(target, grenadeDamage);
			Effects.ApplyForce(target, explosionForceDirection * grenadeExplodeForce);
		}

		Destroy(gameObject);
	}
}
