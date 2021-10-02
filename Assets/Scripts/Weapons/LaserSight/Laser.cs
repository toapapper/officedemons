using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer laserBeam;
    private int maxDistance = 30;
    // Start is called before the first frame update
    void Start()
    {
        laserBeam = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        laserBeam.SetPosition(0, transform.position);


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
		{
			if (hit.collider)
			{
                laserBeam.SetPosition(1, hit.point);
            }
		}
		else
		{
            laserBeam.SetPosition(1, transform.forward * maxDistance);
        }
    }
}
