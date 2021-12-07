using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

// Last Edited: 15/12-07
public class DestructibleObjects : MonoBehaviour
{
    [SerializeField]private GameObject destroyedPrefab;
    [SerializeField] private GameObject particleEffect;
	
    private FieldOfView FOV;
	[SerializeField] private float damage = 10f;
    [SerializeField] private float force = 100;
    [SerializeField] protected List<WeaponEffects> effects;

	public void Start()
	{
        FOV = GetComponentInChildren<FieldOfView>();
	}

    public void Explode()
    {
		AkSoundEngine.PostEvent("Play_Explosion", gameObject);
		Instantiate(particleEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);

		ImpactAgents();

		Instantiate(destroyedPrefab, transform.position, transform.rotation);
		CameraShake.Shake(0.5f, 0.5f);
		Destroy(gameObject);
	}

	private void ImpactAgents()
	{
        if (FOV.VisibleTargets.Count > 0)
        {
			List<GameObject> targetList = FOV.VisibleTargets;

			foreach (GameObject target in targetList)
			{
				if (target.GetComponent<Attributes>().Health > 0)
				{
					if (target.layer != LayerMask.NameToLayer("Destructible"))
					{
						Vector3 explosionForceDirection = target.transform.position - transform.position;
						explosionForceDirection.y = 0;
						explosionForceDirection.Normalize();

						Effects.Damage(target, damage);
						Effects.ApplyForce(target, explosionForceDirection * force);
						Effects.ApplyWeaponEffects(target, effects);
					}
					else
					{
						Effects.Damage(target, damage);
					}
				}
			}

		}
    }



}
