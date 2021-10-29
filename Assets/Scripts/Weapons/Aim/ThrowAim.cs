using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Aim used for the bombard weapons
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
[RequireComponent(typeof(LineRenderer))]
public class ThrowAim : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float gravity;

    private RaycastHit hit;

    [SerializeField]
    public float initialAngle = 45f;
    [SerializeField]
    public float initialVelocity;
    [SerializeField]
    [Range(0.1f, 0.95f)]
    private float initialFriction = 0.7f;
    [SerializeField]
    [Range(0.1f, 0.95f)]
    private float initialBounce = 0.4f;
    [SerializeField]
    private int maxLineLength = 200;
    [SerializeField]
    private float stopVelocity = 0.5f;
    [SerializeField]
    private float maxRayDistance = 0.2f;
    [SerializeField]
    private bool noBouncing;

    private Vector3 currentPosition;
    private Vector3 newPosition;

    private Vector3 velocity;
    private Vector3 u, w;

    private float friction;
    private float bounce;

    void Awake()
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

        while (velocity.magnitude > stopVelocity && lineRenderer.positionCount < maxLineLength)
        {
            currentPosition = newPosition;
            newPosition += velocity * Time.fixedDeltaTime;
            velocity.y -= gravity * Time.fixedDeltaTime;

            if (Physics.Raycast(newPosition, velocity, out hit, maxRayDistance))
            {
				if (noBouncing)
				{
                    newPosition = hit.point;
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
                    break;
                }
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
