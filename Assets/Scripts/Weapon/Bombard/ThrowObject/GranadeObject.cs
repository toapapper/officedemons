using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeObject : MonoBehaviour
{
	private GranadeObject granadeObject;
	[SerializeField]
	private GameObject FOVVisualization;
	FieldOfView fov;
	private int granadeDamage;
	private int granadeExplodeForce;
	[SerializeField]
	private float initialExplodeTime = 10f;
	private float explodeTime;
	private bool detonation;
	private bool isObjectThrown;



	public void CreateGranade(Vector3 position, Vector3 direction, float granadeThrowForce, int granadeExplodeForce, int granadeDamage)
	{
		granadeObject = Instantiate(this, position, Quaternion.LookRotation(direction));
		granadeObject.granadeDamage = granadeDamage;
		granadeObject.granadeExplodeForce = granadeExplodeForce;
		granadeObject.GetComponent<Rigidbody>().AddForce(direction * granadeThrowForce, ForceMode.Impulse);
		granadeObject.explodeTime = initialExplodeTime;
	}

	public void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (detonation)
			{
				explodeTime -= Time.fixedDeltaTime;
				if (explodeTime <= 0)
				{
					Explode();
				}
			}
			else if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				FOVVisualization.SetActive(true);
				detonation = true;
			}
		}
		else
		{
			isObjectThrown = true;
		}
	}

	private void Explode()
	{
		List<GameObject> targetList = GetComponent<FieldOfView>().visibleTargets;

		foreach (GameObject target in targetList)
		{
			Vector3 explosionForceDirection = target.transform.position - transform.position;
			explosionForceDirection.y = 0;
			explosionForceDirection.Normalize();

			target.GetComponent<Actions>().TakeExplosionDamage(granadeDamage, explosionForceDirection * granadeExplodeForce);
		}

		Destroy(gameObject);
	}
}
