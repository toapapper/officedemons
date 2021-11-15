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
    protected float bulletDamage;
    //protected Vector3 bulletHitForce;
    protected float bulletHitForce;
    protected Vector3 bulletDirection;
    protected Bullet bulletObject;

    public List<WeaponEffects> effects;

    public virtual void CreateBullet(Vector3 position, Vector3 direction, float bulletFireForce, float bulletHitForce, float bulletDamage, List<WeaponEffects> effects)
    {
        bulletObject = Instantiate(this, position, Quaternion.LookRotation(direction));
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
        GameManager.Instance.StillCheckList.Add(bulletObject.gameObject);

        bulletObject.effects = effects;
    }

    private void FixedUpdate()
    {
        Vector2 viewpos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewpos.x > 1 || viewpos.x < 0 || viewpos.y > 1 || viewpos.y < 0)
        {
            Destroy(gameObject);
        }
        if (this is Rocket && gameObject)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(bulletDirection * bulletHitForce, ForceMode.Acceleration);
            //Debug.Log("Bulletobj speed: " + gameObject.GetComponent<Rigidbody>().velocity);
            Debug.Log("Bulletobj direction: " + gameObject.GetComponent<Rigidbody>().velocity);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Effects.Damage(collision.gameObject, bulletDamage);
            Effects.ApplyForce(collision.gameObject, bulletDirection * bulletHitForce);

            Effects.ApplyWeaponEffects(collision.gameObject, effects);
        }

        Destroy(gameObject);
    }
}
