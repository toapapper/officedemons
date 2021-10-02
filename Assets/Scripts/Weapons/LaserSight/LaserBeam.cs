using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
	Vector3 pos, dir;

	GameObject laserObject;
	LineRenderer laser;
    public LaserBeam(Vector3 pos, Vector3 dir, Material mat)
	{
		this.laser = new LineRenderer();
		this.laserObject = new GameObject();
		this.laserObject.name = "Laser Beam";
		this.pos = pos;
		this.dir = dir;

		this.laser = laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;

	}
}
