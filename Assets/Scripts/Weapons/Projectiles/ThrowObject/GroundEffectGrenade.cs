using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundEffectGrenade : GrenadeProjectile
{
	protected int maxDistance = 30;

	protected abstract void CreateGroundObject(Vector3 groundObjectPos);

    protected override abstract void ImpactAgents();

    protected void AddToEffectList(GroundEffectObject groundObject)
	{
		List<GameObject> targetList = FOV.VisibleTargets;
		foreach (GameObject target in targetList)
		{
			if (!groundObject.agentsOnGroundEffect.Contains(target) && target.GetComponent<Attributes>().Health > 0)
			{
				groundObject.agentsOnGroundEffect.Add(target);
			}
		}
	}
}
