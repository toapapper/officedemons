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

// Last Edited: 14/11-26
[RequireComponent(typeof(LineRenderer))]
public class ThrowAim : MonoBehaviour
{
	private PlayerMovementController playerMovement;
    private LineRenderer lineRenderer;
	private float gravity;

	private RaycastHit hit;

    [SerializeField]
    public float initialAngle = 45f;
    [SerializeField]
    public float initialSpeed;
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
	private Vector3 targetDirection;
	private float targetSpeed = 10f;

	private Vector3 zeroedPlayer;
	private Vector3 zeroedOrigin;
	private Vector3 zeroedTarget;

	private Vector3 currentArcPosition;
	private Vector3 initialVelocity;
	private Vector3 velocity;
	private Vector3 u, w;

	private float friction;
	private float bounce;

	public bool NoBounceing
	{
		get { return noBouncing; }
		set { noBouncing = value; }
	}
	//public GameObject Target
	//{
	//	get { return target; }
	//}
	public Vector3 TargetDirection
	{
		//get { return targetDirection; }
		set { targetDirection = value; }
	}
	public Vector3 InitialVelocity
	{
		get { return initialVelocity; }
	}

    void Awake()
    {
		playerMovement = GetComponentInParent<PlayerMovementController>();
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
		MoveTarget();
        SetDirection();
		RenderArc();
    }

	private void MoveTarget()
	{
		target.transform.position += targetDirection * targetSpeed * Time.fixedDeltaTime;
	}

    private void SetDirection()
	{
        targetPosition = target.transform.position;
        zeroedPlayer = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        zeroedTarget = new Vector3(targetPosition.x, 0, targetPosition.z);
		player.transform.rotation = Quaternion.LookRotation((zeroedTarget - zeroedPlayer).normalized);
		playerMovement.RotationDirection = player.transform.rotation;
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
		initialVelocity = CalculateVelocity();
		velocity = initialVelocity;

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
}
