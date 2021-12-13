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
        bulletObject = Instantiate(this, position, Quaternion.LookRotation(direction));
        bulletObject.shooter = shooter;
        bulletObject.bulletDamage = bulletDamage;

        bulletObject.bulletHitForce = bulletHitForce;
        bulletObject.bulletDirection = direction;

        //bulletObject.bulletHitForce = direction * bulletHitForce;
        if (this is Rocket)
        {
            AkSoundEngine.PostEvent("Bazooka_Launch", gameObject);
            AkSoundEngine.PostEvent("Play_Bazooka_Shell_Whoosh", gameObject);
            bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.Acceleration);
        }
        else if(this is StapleBullet)
        {
            AkSoundEngine.PostEvent("Play_Stapler", gameObject);
            AkSoundEngine.PostEvent("Bullets_Whiz_By", gameObject);
            bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.VelocityChange);
        }
        else
        {
            AkSoundEngine.PostEvent("Bullets_Whiz_By", gameObject);
            AkSoundEngine.PostEvent("Pistol_shot", gameObject);
            bulletObject.GetComponent<Rigidbody>().AddForce(direction * bulletFireForce, ForceMode.VelocityChange);
            AkSoundEngine.PostEvent("Random_Shell_Casings", gameObject);
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
            AkSoundEngine.PostEvent("Stop_Bazooka_Shell_Whoosh", gameObject);
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
        AkSoundEngine.PostEvent("Stop_Bazooka_Shell_Whoosh", gameObject);
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            AkSoundEngine.PostEvent("Bullet_impact_flesh", gameObject);
            AkSoundEngine.PostEvent("Play_Female_Grunt_Pitched", gameObject);
            Effects.RegularWeaponDamage(collision.gameObject, bulletDamage * (1 + shooter.GetComponentInParent<StatusEffectHandler>().DmgBoost), shooter);
            //Effects.Damage(collision.gameObject, bulletDamage);
            Effects.ApplyForce(collision.gameObject, bulletDirection * bulletHitForce);

            Effects.ApplyWeaponEffects(collision.gameObject, effects);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            AkSoundEngine.PostEvent("Play_FMW_Weapon_Hit10C", gameObject);
            Effects.Damage(collision.gameObject, bulletDamage * (1 + shooter.GetComponentInParent<StatusEffectHandler>().DmgBoost));
        }
        else
        {
            AkSoundEngine.PostEvent("Bullet_Impact_dirt", gameObject);
        }
        //GameManager.Instance.StillCheckList.Remove(gameObject);
        Destroy(gameObject);
    }
}
