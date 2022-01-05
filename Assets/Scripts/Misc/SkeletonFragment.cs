using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class SkeletonFragment : MonoBehaviour
{
    [SerializeField]protected float force = 100f;

    protected Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
