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

// Last Edited: 14/11-24
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

    [SerializeField] private GameObject player;
	[SerializeField] private GameObject target;
    [SerializeField] private GameObject explosion;

	private Vector3 targetStartPosition;
	private Vector3 targetPosition;
	private Vector3 zeroedPlayer;
	private Vector3 zeroedOrigin;
	private Vector3 zeroedTarget;

	private Vector3 currentArcPosition;
	private Vector3 velocity;
	private Vector3 u, w;

	private float friction;
	private float bounce;

	public bool NoBounceing
	{
		get { return noBouncing; }
		set { noBouncing = value; }
	}
	public GameObject Target
	{
		get { return target; }
	}
	public Vector3 Velocity
	{
		get { return velocity; }
	}
	public float ThrowForce
	{
		get { return velocity.magnitude / 2; }
	}

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Physics.gravity.y;
		targetStartPosition = target.transform.localPosition;
	}
	private void OnEnable()
	{
		target.transform.parent = null;
	}

	public void SetExplosionSize(float explosionSize)
	{
		explosion.transform.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
	}

	private void FixedUpdate()
    {
        SetDirection();
		RenderArc();
    }

    private void SetDirection()
	{
        targetPosition = target.transform.position;
        zeroedPlayer = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        zeroedTarget = new Vector3(targetPosition.x, 0, targetPosition.z);
        player.transform.rotation = Quaternion.LookRotation((zeroedTarget - zeroedPlayer).normalized);
        zeroedOrigin = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public Vector3 CalculateVelocity()
    {
        float angleRad = initialAngle * Mathf.Deg2Rad;
        float heightDifference = transform.position.y - targetPosition.y;
        float targetRange = Vector3.Distance(zeroedOrigin, zeroedTarget);

        float speed
            = (Mathf.Sqrt(2) * targetRange * Mathf.Sqrt(-gravity) * Mathf.Sqrt(1 / (Mathf.Sin(2 * angleRad)))) /
              (Mathf.Sqrt((2 * targetRange) + (heightDifference * Mathf.Sin(2 * angleRad) *
              (1 / Mathf.Sin(angleRad)) * (1 / Mathf.Sin(angleRad)))));

        return Quaternion.AngleAxis(-initialAngle, transform.right) * transform.forward * speed;
    }

    void RenderArc()
    {
        velocity = CalculateVelocity();

        friction = initialFriction;
        bounce = initialBounce;

        currentArcPosition = transform.position;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, currentArcPosition);

        while (velocity.magnitude > stopVelocity && lineRenderer.positionCount < maxLineLength)
        {
            currentArcPosition += velocity * Time.fixedDeltaTime;
            velocity.y += gravity * Time.fixedDeltaTime;

            if (Physics.Raycast(currentArcPosition, velocity, out hit, maxRayDistance))
            {
				if (noBouncing)
				{
                    currentArcPosition = hit.point;
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentArcPosition);
                    break;
                }

				currentArcPosition = hit.point;
				u = Vector3.Dot(velocity, hit.normal) * hit.normal;
                w = velocity - u;
                velocity = friction * w - bounce * u;
            }

            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentArcPosition);
        }
        explosion.transform.position = currentArcPosition;
	}

	public void DeActivate()
	{
		target.transform.parent = transform;
		target.transform.localPosition = targetStartPosition;
		gameObject.SetActive(false);
	}










	//   public void CalculateArc()
	//{
	//       Debug.Log("CALCULATE ARC");

	//       //targetPosition = target.transform.position;
	//       //player.transform.rotation = Quaternion.LookRotation((targetPosition - transform.position).normalized);
	//       ////target.transform.position = targetPosition;
	//       velocity = CalculateVelocity();
	//	//velocity = Quaternion.AngleAxis(-initialAngle, transform.right) * transform.forward * speed;

	//	friction = initialFriction;
	//	bounce = initialBounce;

	//	newPosition = transform.position;
	//	lineRenderer.positionCount = 1;
	//	lineRenderer.SetPosition(0, newPosition);

	//	while (velocity.magnitude > stopVelocity && lineRenderer.positionCount < maxLineLength)
	//	{
	//		currentPosition = newPosition;
	//		newPosition += velocity * Time.fixedDeltaTime;
	//		velocity.y += gravity * Time.fixedDeltaTime;

	//		if (Physics.Raycast(newPosition, velocity, out hit, maxRayDistance))
	//		{
	//			if (noBouncing)
	//			{
	//				newPosition = hit.point;
	//				lineRenderer.positionCount += 1;
	//				lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
	//				break;
	//			}

	//			newPosition = hit.point;
	//			u = Vector3.Dot(velocity, hit.normal) * hit.normal;
	//			w = velocity - u;
	//			velocity = friction * w - bounce * u;
	//		}

	//		lineRenderer.positionCount += 1;
	//		lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
	//	}
	//	explodesion.transform.position = newPosition;
	//}







	//IEnumerator ParabolicMotion(Transform projectileTransform, Vector3 target)
	//{
	//	float gravity = 9.82f;
	//	// Calculate the range from the projectile to the target by zero-ing each out in their y-axis
	//	Vector3 zeroedOrigin = new Vector3(projectileTransform.position.x, 0, projectileTransform.position.z);
	//	Vector3 zeroedTarget = new Vector3(target.x, 0, target.z);
	//	Vector3 zeroedDirection = (zeroedTarget - zeroedOrigin).normalized;

	//	float angleRad = initialAngle * Mathf.Deg2Rad;
	//	float heightDifference = projectileTransform.position.y - target.y;
	//	float targetDistance = Vector3.Distance(projectileTransform.position, target);
	//	float targetRange = Vector3.Distance(zeroedOrigin, zeroedTarget);

	//	// Calculate the velocity needed to throw the object to the target at specified angle.
	//	// Velocity can be solved by re-arranging the general equation for parabolic range:
	//	// https://en.wikipedia.org/wiki/Range_of_a_projectile
	//	float projectile_Velocity
	//		= (Mathf.Sqrt(2) * targetRange * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / (Mathf.Sin(2 * angleRad)))) /
	//		  (Mathf.Sqrt((2 * targetRange) + (heightDifference * Mathf.Sin(2 * angleRad) * (1 / Mathf.Sin(angleRad)) * (1 / Mathf.Sin(angleRad)))));

	//	// Extract the X  Y componenent of the velocity
	//	float Vx = projectile_Velocity * Mathf.Cos(angleRad);
	//	float Vy = projectile_Velocity * Mathf.Sin(angleRad);

	//	// Calculate flight time.
	//	float flightDuration = targetRange / Vx;

	//	// Rotate projectile to face the target.
	//	//projectileTransform.rotation = Quaternion.LookRotation(zeroedDirection);
	//	player.transform.rotation = Quaternion.LookRotation(zeroedDirection);

	//	float elapse_time = 0;

	//	while (elapse_time < flightDuration)
	//	{
	//		//transform.Translate(Vx * Time.deltaTime, (Vy - (gravity * elapse_time)) * Time.deltaTime, 0);
	//		transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

	//		elapse_time += Time.deltaTime;

	//		yield return null;
	//	}
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











	//   public Vector3 CalculateInitialVelocity()
	//{
	//	targetPosition = target.transform.position;
	//	Vector3 startPosition = transform.position;

	//       float horizontalDistance = new Vector2(targetPosition.x - startPosition.x, targetPosition.z - startPosition.z).magnitude;
	//       Debug.Log(horizontalDistance);

	//       //float projectile_Velocity = target_Distance / (Mathf.Sin(2 * 45 * Mathf.Deg2Rad) / gravity);

	//       //Original
	//       topPosition = new Vector3((targetPosition.x - startPosition.x) / 2, horizontalDistance / 2, (targetPosition.z - startPosition.z) / 2);

	//	//topPosition = (player.transform.forward * horizontalDistance / 2) + (player.transform.up * horizontalDistance / 3);
	//	Debug.Log(topPosition);
	//       top.transform.position = topPosition;

	//	//Original
	//	//timeToTarget = Mathf.Sqrt(2 * (targetPosition.y - (topPosition.y * targetPosition.x / topPosition.x)) / (gravity * (1 - topPosition.x / targetPosition.x)));

	//	//t2 = sqrt(2 * (P2.y - P1.y*P2.x/P1.x)/(g.y*(1 - P1.x/P2.x)))
	//	timeToTarget = Mathf.Sqrt(2 * (targetPosition.y - (topPosition.y * horizontalDistance / (horizontalDistance / 2))) / (gravity * (1 - (horizontalDistance / 2) / horizontalDistance)));
	//	Debug.Log(timeToTarget);

	//       Vector3 velocity = targetPosition / timeToTarget - (timeToTarget * new Vector3(0 ,gravity, 0)) / 2;

	//       return velocity;



	//       //P1 = point above the net - player position (reached when t=t1)
	//       //P2 = target point - player position (reached when t=t2)
	//       //v = initial velocity vector
	//       //g = gravitational acceleration vector

	//       //General:
	//       //P(t) = t * v + t*t * g/2
	//       //v = P2/t2 - t2 * g/2
	//       //v = P1/t1 - t1 * g/2

	//       //t2 = sqrt(2 * (P2.y - P1.y*P2.x/P1.x)/(g.y*(1 - P1.x/P2.x)))
	//       //v = P2/t2 - t2 * g/2;
	//       //v = P2/t2 - t2 * g/2;

	//       //P2.x/t2 = P1.x/t1
	//       //t1 * P2.x = t2 * P1.x
	//       //t1 = t2 * P1.x/P2.x

	//   }







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
