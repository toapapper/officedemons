using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StapleBullet : Bullet
{
    private bool hasCollided;

    private IEnumerator CountdownTime(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Actions>() != null)
        {
            collision.gameObject.GetComponentInParent<Actions>().TakeBulletDamage(bulletDamage, bulletHitForce);
        }
        if(collision.gameObject.tag != "StapleBullet" && !hasCollided)
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
}
