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

    public bool NoBounceing
	{
		get { return noBouncing; }
		set { noBouncing = value; }
	}

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

    //Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t, Vector3 outDirection)
    //{
    //    float parabolicT = t * 2 - 1;
    //    //start and end are not level, gets more complicated
    //    Vector3 travelDirection = end - start;
    //    Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
    //    Vector3 right = Vector3.Cross(travelDirection, levelDirection);
    //    Vector3 up = outDirection;
    //    Vector3 result = start + t * travelDirection;
    //    result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
    //    return result;
    //}

    //public void CreateParabola(float x1, float x2, float y, float angle)
    //{
    //    var startPoint = new Vector2(x1, y);
    //    var endPoint = new Vector2(x2, y);

    //    var controlPoint = new Vector2(
    //        0.5f * (x1 + x2),
    //        y - 0.5 * (x2 - x1) * Mathf.Tan(angle * Mathf.PI / 180));

    //    var geometry = new StreamGeometry();

    //    using (var context = geometry.Open())
    //    {
    //        context.BeginFigure(startPoint, false, false);
    //        context.QuadraticBezierTo(controlPoint, endPoint, true, false);
    //    }

    //    return geometry;
    //}
}
