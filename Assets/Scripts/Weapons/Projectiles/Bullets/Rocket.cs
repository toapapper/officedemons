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
                    if (target.layer == LayerMask.NameToLayer("Destructible"))
                    {
                        Effects.Damage(target, bulletDamage * (1 + shooter.GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost));
                    }
                    else if (target.tag == "Player" || target.tag == "Enemy" || target.tag == "NPC")
                    {
                        Effects.RegularWeaponDamage(target, bulletDamage * (1 + shooter.GetComponentInParent<Attributes>().statusEffectHandler.DmgBoost), shooter);
                        //Effects.Damage(target, bulletDamage);
                        Effects.ApplyForce(target, (target.transform.position - transform.position).normalized * bulletHitForce);
                    }
                }
            }

            //TODO Explosion
            if (particleEffect)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                AkSoundEngine.PostEvent("Play_Explosion", gameObject);
                CameraShake.Shake(0.5f, 0.5f);
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
