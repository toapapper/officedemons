using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThrowAim : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float gravity;

    private RaycastHit hit;

    [SerializeField]
    private float initialAngle = 45f;
    [SerializeField]
    private float initialVelocity = 5f;
    [SerializeField]
    [Range(0.1f, 0.95f)]
    private float initialFriction = 0.9f;
    [SerializeField]
    [Range(0.1f, 0.95f)]
    private float initialBounce = 0.7f;

    private Vector3 currentPosition;
    private Vector3 newPosition;

    private Vector3 velocity;
    private Vector3 u, w;

    private float friction;
    private float bounce;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics.gravity.y);
    }

    private void FixedUpdate()
    {
        RenderArc();
    }

    void RenderArc()
    {
        velocity = Quaternion.AngleAxis(-initialAngle, transform.right) * transform.forward * initialVelocity;
        friction = initialFriction;
        bounce = initialBounce;

        newPosition = transform.position;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, newPosition);

        while (velocity.magnitude > 0.1f && lineRenderer.positionCount < 200)
        {
            currentPosition = newPosition;
            newPosition += velocity * Time.fixedDeltaTime;
            velocity.y -= gravity * Time.fixedDeltaTime;

            Debug.Log(Time.fixedDeltaTime);

            if (Physics.Raycast(newPosition, velocity, out hit, /*Mathf.Abs((currentPosition - newPosition).magnitude + */0.1f))
            {
                newPosition = hit.point;
                u = Vector3.Dot(velocity, hit.normal) * hit.normal;
                w = velocity - u;
                velocity = friction * w - bounce * u;
            }

            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
        }

    }
}
