using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// The staple object/projectile
/// 
/// </para>
///   
///  <para>
///  Author: Jonas Lundin
///  
/// </para>
///  
/// </summary>

// Last Edited: 14/10-21
public class StapleBullet : Bullet
{
    private bool hasCollided;

    private IEnumerator CountdownTime(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponentInChildren<ParticleSystem>() != null)
        {
            Hit();

        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Effects.Damage(collision.gameObject, bulletDamage);
            Effects.ApplyForce(collision.gameObject, bulletHitForce);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag != "Projectile" && !hasCollided)
        {
            hasCollided = true;
            if (Random.value > 0.7)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //gameObject.transform.rotation = Quaternion.Euler(collision.GetContact(0).normal);
            }
        }
        StartCoroutine(CountdownTime(3));
    }
    void Hit()
    {
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
    }
}
