using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class fallingRockScript : MonoBehaviour
{
    private Vector3 startPosition;
    private float fallSpeed = 0f;

    [Tooltip("positive gravity causes things to fall downwards")]
    [SerializeField] private float gravity = 5f;

    [Tooltip("The max velocity of the stone. sign agnostic")]
    [SerializeField] private float maxFallSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        BoxCollider bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.fallSpeed -= this.gravity * Time.deltaTime;
        if(Mathf.Abs(this.fallSpeed) > Mathf.Abs(maxFallSpeed))
        {
            this.fallSpeed = Mathf.Sign(this.fallSpeed) * Mathf.Abs(maxFallSpeed);
        }

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.fallSpeed * Time.deltaTime, this.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bottom_of_world"))
        {
            this.transform.position = startPosition;
            this.fallSpeed = 0;
        }
    }
}
