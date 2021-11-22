using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Abstract class for all grenades also creating groundEffects on impact
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 15-11-19
public abstract class GroundEffectGrenade : GrenadeProjectile
{
	protected int maxDistance = 30;

	protected abstract void CreateGroundObject(Vector3 groundObjectPos);

    protected override abstract void ImpactAgents();

 //   protected void AddToEffectList(GroundEffectObject groundObject)
	//{
	//	List<GameObject> targetList = FOV.VisibleTargets;
	//	foreach (GameObject target in targetList)
	//	{
	//		if (!groundObject.agentsOnGroundEffect.Contains(target) && target.GetComponent<Attributes>().Health > 0)
	//		{
	//			groundObject.agentsOnGroundEffect.Add(target);
	//		}
	//	}
	//}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			CreateGroundObject(collision.contacts[0].point);
		}
		else
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, LayerMask.GetMask("Ground")))
			{
				CreateGroundObject(hit.point);
			}
		}
	}
}
