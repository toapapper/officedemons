using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{
    [Tooltip("max force per axis")]
    [SerializeField]private float maxForce = 100f;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < bodies.Length; i++)
        {
            float x = (Random.value - .5f) * maxForce * 2;
            float y = (Random.value - .5f) * maxForce * 2;
            float z = (Random.value - .5f) * maxForce * 2;
            Vector3 v = new Vector3(x, y, z);

            bodies[i].AddForce(v);
        }
    }
}
