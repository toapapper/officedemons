using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The bullet object/projectile
/// </para>
///
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-28
public class Bullet : MonoBehaviour
{
    protected GameObject shooter;
    protected float bulletDamage;
    //protected Vector3 bulletHitForce;
    protected float bulletHitForce;
    protected Vector3 bulletDirection;
    private Bullet bulletObject;

    public List<StatusEffectType> effects;

    public void CreateBullet(GameObject shooter, Vector3 position, Vector3 direction, float bulletFireForce, float bulletHitForce, float bulletDamage, List<StatusEffectType> effects)
    {
        AkSoundEngine.PostEvent("Play_ShotSFX", gameObject);
        bulletObject = Instantiate(this, position, Quaternion.LookRotation(direction));
        bulletObject.shooter = shooter;
        bulletObject.bulletDamage = bulletDamage;

        bulletObject.bulletHitForce = bulletFireForce;
        bulletObject.bulletDirection = direction;

        //bulletObject.bulletHitForce = direction * bulletHitForce;
        if (this is Rocket)
        {
            bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.Acceleration);
        }
        else
        {
            bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.VelocityChange);
        }
        //GameManager.Instance.StillCheckList.Add(bulletObject.gameObject);

        bulletObject.effects = effects;
    }

    private void FixedUpdate()
    {
        Vector2 viewpos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewpos.x > 1 || viewpos.x < 0 || viewpos.y > 1 || viewpos.y < 0)
        {
            //GameManager.Instance.StillCheckList.Remove(gameObject);
            Destroy(gameObject);
        }
        if (this is Rocket && gameObject)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(bulletDirection * bulletHitForce, ForceMode.Acceleration);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("projectile collision");

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Effects.RegularWeaponDamage(collision.gameObject, bulletDamage * (1 + shooter.GetComponentInParent<StatusEffectHandler>().DmgBoost), shooter);
            //Effects.Damage(collision.gameObject, bulletDamage);
            Effects.ApplyForce(collision.gameObject, bulletDirection * bulletHitForce);

            Effects.ApplyWeaponEffects(collision.gameObject, effects);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            Effects.Damage(collision.gameObject, bulletDamage * (1 + shooter.GetComponentInParent<StatusEffectHandler>().DmgBoost));
        }
        AkSoundEngine.PostEvent("Play_FMW_Weapon_Hit10C", gameObject);
        //GameManager.Instance.StillCheckList.Remove(gameObject);
        Destroy(gameObject);
    }
}
