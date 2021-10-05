using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code by: Johan Melkersson
/// </summary>
public class Laser : MonoBehaviour
{
    private LineRenderer laserBeam;
    [SerializeField]
    private int maxLaserDistance = 30;
    [SerializeField]
    private LayerMask ignoreLayer;

    void Start()
    {
        laserBeam = GetComponent<LineRenderer>();
    }

	private void Update()
	{
        laserBeam.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxLaserDistance, ~ignoreLayer))
        {
            laserBeam.SetPosition(1, hit.point);
        }
        else
        {
            laserBeam.SetPosition(1, transform.position + transform.forward * maxLaserDistance);
        }

    }
}
