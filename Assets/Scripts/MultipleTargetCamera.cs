using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script by Jonas
/// </summary>
[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public Vector3 offset;
    private Vector3 velocity;
    public float smoothTime = .5f;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (PlayerManager.players == null)
        {
            return;
        }
        else if (PlayerManager.players.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

		//Vector3 newPosition = centerPoint + offset;

		transform.parent.position = Vector3.SmoothDamp(transform.parent.position, centerPoint, ref velocity, smoothTime);
	}

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(PlayerManager.players[0].transform.position, Vector3.zero);
        for (int i = 0; i < PlayerManager.players.Count; i++)
        {
            bounds.Encapsulate(PlayerManager.players[i].transform.position);
        }
        if (bounds.size.x > bounds.size.y)
        {
            return bounds.size.x;
        }
        else
        {
            return bounds.size.y;
        }

    }
    Vector3 GetCenterPoint()
    {
        if (PlayerManager.players.Count == 1)
        {
            return PlayerManager.players[0].transform.position;
        }

        var bounds = new Bounds(PlayerManager.players[0].transform.position, Vector3.zero);
        for (int i = 0; i < PlayerManager.players.Count; i++)
        {
            bounds.Encapsulate(PlayerManager.players[i].transform.position);
        }

        return bounds.center;
    }
}
