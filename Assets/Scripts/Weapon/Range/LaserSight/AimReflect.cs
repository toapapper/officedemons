using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Aim that can reflect and bounce of walls (never used)
/// </para>
///   
///  <para>
///  Author: Johan Melkersson
/// </para>
/// </summary>

// Last Edited: 14/10-21
public class AimReflect : MonoBehaviour
{
    public int reflections;
    public float maxLength;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;
    

    private Vector3 startPosition;
    private Vector3 currentArcPosition;
    private Vector3 currentDirection;
    private Vector3 currentDirectionX, currentDirectionY;


    LineRenderer lineRenderer;

    public float velocity;
    public float angle;
    public int resolution = 10;

    private float gravity;
    private float radianAngle;


    float vx;
    float vy;




    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics.gravity.y);
    }



    private void Update()
    {
        //ray = new Ray(transform.position, transform.forward + Vector3.up);

        Vector3 rot = Quaternion.AngleAxis(-angle, Vector3.right) * Vector3.forward;
        ray = new Ray(transform.position, rot);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLenght = maxLength;

        for (int i = 0; i < reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLenght))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLenght -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLenght);
            }
        }


    }
}
