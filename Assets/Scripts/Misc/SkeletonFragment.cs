using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class SkeletonFragment : MonoBehaviour
{
    [SerializeField]protected float force = 100f;

    protected Rigidbody rb;

    [SerializeField] protected float lifeTimeMax = 20f;
    [SerializeField] protected float lifeTimeMin = 15f;

    protected float lifeTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lifeTime = Random.Range(lifeTimeMin, lifeTimeMax);
    }

    private void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb == null)
            return;

        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
        {
            Vector3 direction = transform.position - collision.transform.position;
            direction.y = 0;
            direction.Normalize();
            direction *= force;
            direction.y = 2;//slightly upwards

            rb.AddForce(direction);
        }
    }

}
