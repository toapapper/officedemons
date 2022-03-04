using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{


    [SerializeField] protected float duration = 10f;
    [SerializeField] protected FieldOfView FOV;
	//anyway. theses should be set in the prefab and have no effect if changed after spawn
    [SerializeField] protected List<StatusEffectType> effects;
    [SerializeField] protected float force = 50f;
	[SerializeField] protected float damage = 20f;

	[Header("Set the size in the FOV-script!")]
	[SerializeField] protected float uselessFloatThatOnlyExistsForTheHeaderToBeVisible = 69.420f;

	private bool exploded0 = true;
	private bool exploded1 = true;


	public GameObject SpawnerAgent = null;


    private void Explode()
    {
		Debug.Log("Explode!");
		FOV.FindVisibleTargets();

		if (FOV.VisibleTargets.Count > 0)
		{
			foreach (GameObject target in FOV.VisibleTargets)
			{
				if (target.GetComponent<Attributes>() != null && target.GetComponent<Attributes>().Health > 0)
				{
					if(SpawnerAgent != null && SpawnerAgent.GetComponent<Attributes>() != null)
                    {
						Effects.Damage(target, damage, SpawnerAgent);
                    }
                    else
                    {
						Effects.Damage(target, damage);
                    }


					if (target.layer != LayerMask.NameToLayer("Destructible"))
					{
						Vector3 explosionForceDirection = target.transform.position - transform.position;
						explosionForceDirection.y = 0;
						explosionForceDirection.Normalize();

						Effects.ApplyForce(target, explosionForceDirection * force);
						Effects.ApplyWeaponEffects(target, SpawnerAgent, effects);
					}
				}
			}
		}
	}
    
    // Start is called before the first frame update
    void Start()
    {
		if(FOV == null)
        {
			FOV = GetComponent<FieldOfView>();
        }

		Explode();
		Destroy(gameObject, duration);
	}

}
