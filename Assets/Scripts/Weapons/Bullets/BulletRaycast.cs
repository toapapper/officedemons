using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletRaycast
{
    public static void Shoot(Vector3 shootPosition, Vector3 shootDirection, int maxDistance, int bulletDamage)
	{
		RaycastHit hit;
		if (Physics.Raycast(shootPosition, shootDirection, out hit, maxDistance))
		{
			if (hit.collider != null)
			{
				Actions targetActions = hit.collider.gameObject.GetComponent<Actions>();
				if(targetActions != null)
				{
					targetActions.TakeBulletDamage(bulletDamage, shootPosition);
				}
			}
		}
	}
}
