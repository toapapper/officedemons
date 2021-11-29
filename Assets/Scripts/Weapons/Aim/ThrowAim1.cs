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
public class ThrowAim1 : MonoBehaviour
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


    //[SerializeField]private GameObject target;
    //private Vector3[] positions = new Vector3[50];


    //P1 = point above the net - player position(reached when t= t1)
    //P2 = target point - player position(reached when t= t2)
    //v = initial velocity vector
    //g = gravitational acceleration vector

    //float gravity = 9.82f;
    //Vector3 start = transform.position;

    //private Vector3 gravity;
    [SerializeField] private GameObject player;
	[SerializeField] private GameObject top;
	[SerializeField] private GameObject target;
    [SerializeField] private GameObject explodesion;
    //[SerializeField] private GameObject lookAt;
    private Vector3 topPosition = new Vector3();
    //private Vector3 topPosition = new Vector3();
    private Vector3 targetPosition = new Vector3();
    private float timeToTarget = new float();

    //Vector3 velocity = new Vector3();







    public bool NoBounceing
	{
		get { return noBouncing; }
		set { noBouncing = value; }
	}

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
		//gravity = Mathf.Abs(Physics.gravity.y);
        //gravity = Physics.gravity;
        gravity = Physics.gravity.y;
    }

    private void FixedUpdate()
    {
        RenderArc();
    }



    //  Debug.Log("HEJ");
    ////velocity = Quaternion.AngleAxis(-initialAngle, transform.right) * transform.forward * initialVelocity;
    ////velocity = CalculateInitialVelocity();


    //      //velocity = Quaternion.AngleAxis(-initialAngle, transform.right) * transform.forward * CalculateInitialVelocity().magnitude;

    //      Vector3 v = CalculateInitialVelocity();
    //  Vector3 playerLookDir = new Vector3(v.x, 0, v.z).normalized;
    //  player.transform.rotation = Quaternion.FromToRotation(player.transform.forward, playerLookDir);
    //      //player.transform.rotation = Quaternion.Euler(v.normalized);
    //      velocity = Quaternion.AngleAxis(-Vector3.Angle(v.normalized, transform.forward), transform.right) * transform.forward* v.magnitude;
    //
    void RenderArc()
    {

        //velocity = Quaternion.AngleAxis(-initialAngle, transform.right) * transform.forward * initialVelocity;
        //velocity = CalculateInitialVelocity();
        //velocity = Quaternion.AngleAxis(-Vector3.Angle(v.normalized, transform.forward), transform.right) * transform.forward* v.magnitude;

        Debug.Log("RENDER ARC");
        //targetPosition = target.transform.position;
        //player.transform.LookAt(new Vector3(targetPosition.x, player.transform.position.y, targetPosition.z));

        Debug.Log("FROM DIRECTION " + (transform.forward).normalized);
        Debug.Log("TO DIRECTION " + (target.transform.position - transform.position).normalized);
		player.transform.rotation *= Quaternion.FromToRotation(player.transform.forward, (target.transform.position - transform.position).normalized);
		velocity = CalculateInitialVelocity();
        
		//player.transform.rotation = Quaternion.FromToRotation(player.transform.forward, new Vector3(velocity.x, 0, velocity.z).normalized);

		//Vector3 v = CalculateInitialVelocity();
		//      player.transform.rotation = Quaternion.FromToRotation(player.transform.forward, new Vector3(v.x, 0, v.z).normalized);
		//      velocity = Quaternion.AngleAxis(-Vector3.Angle(v.normalized, transform.forward), transform.right) * transform.forward * v.magnitude;

		Debug.Log(velocity);
        friction = initialFriction;
        bounce = initialBounce;

        newPosition = transform.position;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, newPosition);

        while (velocity.magnitude > stopVelocity && lineRenderer.positionCount < maxLineLength)
        {
            currentPosition = newPosition;
            newPosition += velocity * Time.fixedDeltaTime;

            //velocity.y -= gravity * Time.fixedDeltaTime;
            //velocity += gravity * Time.fixedDeltaTime;
            velocity.y += gravity * Time.fixedDeltaTime;

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
        explodesion.transform.position = newPosition;
    }








    public Vector3 CalculateInitialVelocity()
	{
		targetPosition = target.transform.position;
		Vector3 startPosition = transform.position;

        float horizontalDistance = new Vector2(targetPosition.x - startPosition.x, targetPosition.z - startPosition.z).magnitude;
        Debug.Log(horizontalDistance);

        //float projectile_Velocity = target_Distance / (Mathf.Sin(2 * 45 * Mathf.Deg2Rad) / gravity);

        //Original
        topPosition = new Vector3((targetPosition.x - startPosition.x) / 2, horizontalDistance / 2, (targetPosition.z - startPosition.z) / 2);

		//topPosition = (player.transform.forward * horizontalDistance / 2) + (player.transform.up * horizontalDistance / 3);
		Debug.Log(topPosition);
        top.transform.position = topPosition;

		//Original
		//timeToTarget = Mathf.Sqrt(2 * (targetPosition.y - (topPosition.y * targetPosition.x / topPosition.x)) / (gravity * (1 - topPosition.x / targetPosition.x)));

		//t2 = sqrt(2 * (P2.y - P1.y*P2.x/P1.x)/(g.y*(1 - P1.x/P2.x)))
		timeToTarget = Mathf.Sqrt(2 * (targetPosition.y - (topPosition.y * horizontalDistance / (horizontalDistance / 2))) / (gravity * (1 - (horizontalDistance / 2) / horizontalDistance)));
		Debug.Log(timeToTarget);

        Vector3 velocity = targetPosition / timeToTarget - (timeToTarget * new Vector3(0 ,gravity, 0)) / 2;

        return velocity;



        //P1 = point above the net - player position (reached when t=t1)
        //P2 = target point - player position (reached when t=t2)
        //v = initial velocity vector
        //g = gravitational acceleration vector

        //General:
        //P(t) = t * v + t*t * g/2
        //v = P2/t2 - t2 * g/2
        //v = P1/t1 - t1 * g/2

        //t2 = sqrt(2 * (P2.y - P1.y*P2.x/P1.x)/(g.y*(1 - P1.x/P2.x)))
        //v = P2/t2 - t2 * g/2;
        //v = P2/t2 - t2 * g/2;

        //P2.x/t2 = P1.x/t1
        //t1 * P2.x = t2 * P1.x
        //t1 = t2 * P1.x/P2.x

    }


    //IEnumerator ParabolicMotion(Transform projectileTransform, Vector3 target)
    //{
    //    // Calculate the range from the projectile to the target by zero-ing each out in their y-axis
    //    Vector3 zeroedOrigin = new Vector3(projectileTransform.position.x, 0, projectileTransform.position.z);
    //    Vector3 zeroedTarget = new Vector3(target.x, 0, target.z);
    //    Vector3 zeroedDirection = (zeroedTarget - zeroedOrigin).normalized;

    //    float angleRad = initialAngle * Mathf.Deg2Rad;
    //    float heightDifference = projectileTransform.y - target.y;
    //    float targetDistance = Vector3.Distance(projectileTransform.position, target);
    //    float targetRange = Vector3.Distance(zeroedOrigin, zeroedTarget);

    //    // Calculate the velocity needed to throw the object to the target at specified angle.
    //    // Velocity can be solved by re-arranging the general equation for parabolic range:
    //    // https://en.wikipedia.org/wiki/Range_of_a_projectile
    //    float projectile_Velocity
    //        = (Mathf.Sqrt(2) * targetRange * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / (Mathf.Sin(2 * angleRad)))) /
    //          (Mathf.Sqrt((2 * targetRange) + (heightDifference * Mathf.Sin(2 * angleRad) * (1 / Mathf.Sin(angleRad)) * (1 / Mathf.Sin(angleRad)))));

    //    // Extract the X  Y componenent of the velocity
    //    float Vx = projectile_Velocity * Mathf.Cos(angleRad);
    //    float Vy = projectile_Velocity * Mathf.Sin(angleRad);

    //    // Calculate flight time.
    //    float flightDuration = targetRange / Vx;

    //    // Rotate projectile to face the target.
    //    projectileTransform.rotation = Quaternion.LookRotation(zeroedDirection);

    //    float elapse_time = 0;

    //    while (elapse_time < flightDuration)
    //    {
    //        transform.Translate(Vx * Time.deltaTime, (Vy - (gravity * elapse_time)) * Time.deltaTime, 0);
    //        //transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

    //        elapse_time += Time.deltaTime;

    //        yield return null;
    //    }
    //}










    //
    //  public Vector3 CalculateInitialVelocity()
    //  {
    //      targetPosition = target.transform.position;
    //      //Vector3 playerLookPos = new Vector3(targetPosition.x, player.transform.position.y, targetPosition.z);


    //      //player.transform.LookAt(playerLookPos.transform.position);

    //      Vector3 startPosition = transform.position;
    //      //topPosition = 
    //      //topPosition = transform.position;


    //      //float horizontalDistance = new Vector2(targetPosition.x - startPosition.x, targetPosition.z - startPosition.z).magnitude;
    //      float horizontalDistance = targetPosition.magnitude - transform.position.magnitude;

    //      //float horizontalDistance = new Vector2(targetPosition.x - startPosition.x, targetPosition.z - startPosition.z).magnitude;
    //      Debug.Log(horizontalDistance);
    //      topPosition = new Vector3((targetPosition.x - startPosition.x) / 2, horizontalDistance / 3, (targetPosition.z - startPosition.z) / 2);
    //      Debug.Log(topPosition);


    //      //      Debug.Log(topPosition);
    //      //      Debug.Log(targetPosition);
    //      //      Debug.Log(gravity);
    //      //      float step1;
    //      //      step1 = topPosition.x / targetPosition.x;
    //      //      Debug.Log(step1);
    //      //      float step2 = 1 - step1;
    //      //      Debug.Log(step2);
    //      //      float step3 = step2 * gravity;
    //      //      Debug.Log(step3);

    //      //      float step4 = topPosition.y * targetPosition.x;
    //      //      Debug.Log(step4);
    //      //      float startX = topPosition.x;
    //      //      Debug.Log(startX);
    //      //double step5 = step4 / startX;
    //      ////float step5 = 0f / 0f;
    //      //Debug.Log(step5);

    //      //      float test = 2 * (targetPosition.y - (topPosition.y * targetPosition.x / topPosition.x)) / (gravity * (1 - topPosition.x / targetPosition.x));

    //      timeToTarget = Mathf.Sqrt(2 * (targetPosition.y - (topPosition.y * targetPosition.z / topPosition.z)) / (gravity * (1 - topPosition.z / targetPosition.z)));

    //      //float time;
    //      //time = Mathf.Sqrt(2 * (targetPosition.y - (topPosition.y * targetPosition.x / topPosition.x)) / (gravity * (1 - topPosition.x / targetPosition.x)));
    //      //Debug.Log(time);

    //      Debug.Log(timeToTarget);

    //      //float t1 = t2 * (startPosition.x / targetPosition.x);
    //      Vector3 velocity = targetPosition / timeToTarget - timeToTarget * /*gravity*/new Vector3(0, gravity, 0) / 2;

    //      return velocity;



    //      //P1 = point above the net - player position (reached when t=t1)
    //      //P2 = target point - player position (reached when t=t2)
    //      //v = initial velocity vector
    //      //g = gravitational acceleration vector

    //      //General:
    //      //P(t) = t * v + t*t * g/2
    //      //v = P2/t2 - t2 * g/2
    //      //v = P1/t1 - t1 * g/2

    //      //t2 = sqrt(2 * (P2.y - P1.y*P2.x/P1.x)/(g.y*(1 - P1.x/P2.x)))
    //      //v = P2/t2 - t2 * g/2;
    //      //v = P2/t2 - t2 * g/2;

    //      //P2.x/t2 = P1.x/t1
    //      //t1 * P2.x = t2 * P1.x
    //      //t1 = t2 * P1.x/P2.x

    //  }















    //   private Vector3 CalculateQuadraticBeizerPoint(float time, Vector3 startPosition, Vector3 endPosition, Vector3 controlPosition)
    //{
    //       float u = 1 - time;
    //       float tt = time * time;
    //       float uu = u * u;
    //       Vector3 currentPosition = uu * startPosition + 2 * u * time * endPosition + tt * controlPosition;

    //       return currentPosition;
    //   }

    //   private void DrawQuadraticCurve()
    //{
    //       float newPoints = lineRenderer.positionCount;

    //       for (int i = 0; i < newPoints + 1; i++)
    //	{
    //           float t = i / (float)newPoints;
    //           positions[i-1] = CalculateQuadraticBeizerPoint(t,transform.position, target.transform.position,)
    //       }
    //}
}
