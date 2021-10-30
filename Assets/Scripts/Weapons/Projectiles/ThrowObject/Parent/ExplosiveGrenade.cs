using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGrenade : GrenadeObject
{
	//public void CreateGrenade(Vector3 position, Vector3 direction, float grenadeThrowForce, float grenadeExplodeForce, float grenadeDamage, List<WeaponEffects> effects)
	//{
	//	grenadeObject = Instantiate(this, position, Quaternion.LookRotation(direction));
	//	grenadeObject.GetComponent<FieldOfView>().ViewRadius = explodeRadius;
	//	grenadeObject.grenadeDamage = grenadeDamage;
	//	grenadeObject.grenadeExplodeForce = grenadeExplodeForce;
	//	grenadeObject.GetComponent<Rigidbody>().AddForce(direction * grenadeThrowForce, ForceMode.Impulse);
	//	grenadeObject.explodeTime = initialExplodeTime;
	//	GameManager.Instance.StillCheckList.Add(grenadeObject.gameObject);

	//	this.effects = effects;
	//}

	protected override void FixedUpdate()
	{
		if (isObjectThrown)
		{
			if (GetComponent<Rigidbody>().velocity.magnitude < 0.5f)
			{
				base.SetExplosion();
				//FOVVisualization.SetActive(true);
				//base.StartCoroutine(CountdownTime(explodeTime));
			}
		}
		else
		{
			isObjectThrown = true;
		}
	}

	//private IEnumerator CountdownTime(float time)
	//{
	//	yield return new WaitForSeconds(time);
	//	Explode();
	//}

	//protected override void Explode()
	//{
	//	List<GameObject> targetList = GetComponent<FieldOfView>().VisibleTargets;

	//	foreach (GameObject target in targetList)
	//	{
	//		Vector3 explosionForceDirection = target.transform.position - transform.position;
	//		explosionForceDirection.y = 0;
	//		explosionForceDirection.Normalize();

	//		Effects.Damage(target, grenadeDamage);
	//		Effects.ApplyForce(target, explosionForceDirection * grenadeExplodeForce);
	//		Effects.ApplyWeaponEffects(target, effects);
	//	}

	//	Destroy(gameObject);
	//}
}
