using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer laserBeam;
    private int maxDistance = 30;
    [SerializeField]
    private LayerMask ignoreLayer;
    [SerializeField]
    private GameObject muzzle;

    // Start is called before the first frame update
    void Start()
    {
        laserBeam = GetComponent<LineRenderer>();
    }

	private void Update()
	{
        laserBeam.SetPosition(0, muzzle.transform.position);


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            laserBeam.SetPosition(1, hit.point);
            if (hit.collider)
            {
                
            }
        }
        else
        {
            laserBeam.SetPosition(1, transform.position + transform.forward * maxDistance);
        }

    }
}
