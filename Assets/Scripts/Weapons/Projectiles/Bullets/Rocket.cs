using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The rocket object/projectile
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-28
public class Rocket : Bullet
{
    private List<GameObject> targetList = new List<GameObject>();
    [SerializeField]
    private GameObject particleEffect;
    private bool isExploded = false;

	protected override void OnCollisionEnter(Collision collision)
	{
		if (!isExploded)
		{
            isExploded = true;

            if (targetList.Count > 0)
            {
                foreach (GameObject target in targetList)
                {
                    if (target.tag == "Player" || target.tag == "Enemy")
                    {
                        Effects.RegularWeaponDamage(target, bulletDamage * (1 + shooter.GetComponentInParent<StatusEffectHandler>().DmgBoost), shooter);
                        //Effects.Damage(target, bulletDamage);
                        Effects.ApplyForce(target, (target.transform.position - transform.position).normalized * bulletHitForce);
                    }
                    else if(target.tag == "CoverObject")
					{
                        Effects.Damage(target, bulletDamage * (1 + shooter.GetComponentInParent<StatusEffectHandler>().DmgBoost));
					}
                }
            }

            //TODO Explosion
            if (particleEffect)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                AkSoundEngine.PostEvent("Play_Explosion", gameObject);
            }
            //gameObject.GetComponentInChildren<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "CoverObject")
        {
            if (!targetList.Contains(other.gameObject))
            {
                targetList.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetList.Contains(other.gameObject))
        {
            targetList.Remove(other.gameObject);
        }
        //if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "CoverObject")
        //{
        //    targetList.Remove(other.gameObject);
        //}
    }
}
