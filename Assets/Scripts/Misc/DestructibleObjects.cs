using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
///
/// </summary>

// Last Edited: 15/12-21
public class DestructibleObjects : MonoBehaviour
{
	public bool destroyed;
    public GameObject destroyer = null;

    [SerializeField]private GameObject destroyedPrefab;
    [SerializeField] private GameObject particleEffect;

    private FieldOfView FOV;
	[SerializeField] private float damage = 10f;
    [SerializeField] private float force = 100;
    [SerializeField] protected List<StatusEffectType> effects;

    private GameObject preExplosionEffect;
    private GameObject preExplosionEffectInstance;

	public void Start()
	{
        FOV = GetComponentInChildren<FieldOfView>();
        if (FOV == null)
        {
            FOV = GetComponent<FieldOfView>();
        }

        preExplosionEffect = Resources.Load<GameObject>("preExplosionSmoke");
	}

    public void ExplodeWithDelay(float delay = .3f)
    {
        preExplosionEffectInstance = Instantiate(preExplosionEffect, transform.position, Quaternion.identity);
        explodeDelay(delay);
    }

    private async void explodeDelay(float delay)
    {
        await Task.Delay((int)(delay * 1000));
        Explode();
    }

    public void Explode()
    {
        Destroy(preExplosionEffectInstance);

        if(particleEffect != null)
        {
		    AkSoundEngine.PostEvent("Play_Explosion", gameObject);
		    Instantiate(particleEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        }

        if (!FOV.isActiveAndEnabled)
        {
            FOV.enabled = !FOV.enabled;
        }

        FOV.FindVisibleTargets();
        ImpactAgents();


        if (destroyedPrefab == null)
        {
            MeshRenderer mrOnMe = GetComponent<MeshRenderer>();
            if (mrOnMe != null)
            {
                foreach(Material m in mrOnMe.materials)
                {
                    m.color = Color.black;
                }
            }

            foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                foreach (Material m in mr.materials)
                {
                    m.color = Color.black;
                }
            }
            destroyed = true;
            // Spawn smoking particle effect
        }
        else
        {
            GameObject destroyedObject = Instantiate(destroyedPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
			destroyedObject.transform.parent = GameObject.Find("DestructibleObjects").transform;
		}

        if(this.transform.Find("Lights"))
        {
            if (this.transform.Find("Lights").gameObject.GetComponent<EmergencyLights>())
            {
                this.transform.Find("Lights").gameObject.GetComponent<EmergencyLights>().enabled = false;
                Destroy(this.transform.Find("Lights").gameObject);
            }
        }
        else if (this.transform.Find("AmbulanceLights"))
        {
            if (this.transform.Find("AmbulanceLights").gameObject.GetComponent<EmergencyLights>())
            {
                this.transform.Find("AmbulanceLights").gameObject.GetComponent<EmergencyLights>().enabled = false;
                Destroy(this.transform.Find("AmbulanceLights").gameObject);

            }
        }

        CameraShake.Shake(0.5f, 0.5f);
    }

	private void ImpactAgents()
	{
        if (FOV.VisibleTargets.Count > 0)
        {
			List<GameObject> targetList = FOV.VisibleTargets;

			foreach (GameObject target in targetList)
			{
				if (target.GetComponent<Attributes>() != null && target.GetComponent<Attributes>().Health > 0)
				{
					if (target.layer != LayerMask.NameToLayer("Destructible"))
					{
						Vector3 explosionForceDirection = target.transform.position - transform.position;
						explosionForceDirection.y = 0;
						explosionForceDirection.Normalize();

						Effects.Damage(target, damage, destroyer);
						Effects.ApplyForce(target, explosionForceDirection * force, destroyer);
						Effects.ApplyWeaponEffects(target, null, effects);
					}
					else
					{
						Effects.Damage(target, damage, destroyer);
					}
				}
			}
		}
    }
}
