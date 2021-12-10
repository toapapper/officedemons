using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSmash : MonoBehaviour
{
    [SerializeField]
    private GameObject particleEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {

        Instantiate(particleEffect, transform.position, transform.rotation * Quaternion.Euler(0, 180, 0));
        Destroy(gameObject);
    }
}
