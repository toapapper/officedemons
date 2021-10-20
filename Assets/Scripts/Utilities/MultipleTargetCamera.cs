using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Camera script that focuses on the midpoint between all players, and enemies when in an encounter
/// 
/// </para>
///   
///  <para>
///  Author: Jonas
///  
/// </para>
///  
/// </summary>

// Last Edited: 14/10-21

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public Vector3 offset;
    private Vector3 velocity;
    public float smoothTime = .5f;
    public float minZoom = 13f;
    public float maxZoom = 7f;
    public float zoomLimiter = 30f;

    private Camera cam;
    private List<GameObject> objectsInCamera;
    public List<GameObject> ObjectsInCamera
    {
        get { return objectsInCamera; }
        set { objectsInCamera = value; }
    }

    private void Start()
    {
        cam = GetComponent<Camera>();

        objectsInCamera = PlayerManager.players;
    }

    private void LateUpdate()
    {
        if (objectsInCamera == null)
        {
            return;
        }
        else if (objectsInCamera.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    /// <summary>
    /// Adjust camera zoom by changing the orthographic size
    /// </summary>
    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    /// <summary>
    /// Move the camera
    /// </summary>
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

		//Vector3 newPosition = centerPoint + offset;

		transform.parent.position = Vector3.SmoothDamp(transform.parent.position, centerPoint, ref velocity, smoothTime);
	}

    /// <summary>
    /// Returns the greatest distance between all objects within the bounds
    /// </summary>
    /// <returns></returns>
    private float GetGreatestDistance()
    {
        var bounds = new Bounds(objectsInCamera[0].transform.position, Vector3.zero);
        for (int i = 0; i < objectsInCamera.Count; i++)
        {
            bounds.Encapsulate(objectsInCamera[i].transform.position);
        }
        DrawBounds(bounds, 0);
        if (bounds.size.x > bounds.size.z)
        {
            return bounds.size.x;
        }
        else
        {
            return bounds.size.z;
        }

    }

    /// <summary>
    /// Return the center point of the camera bounds
    /// </summary>
    /// <returns></returns>
    Vector3 GetCenterPoint()
    {
        if (objectsInCamera.Count == 1)
        {
            return objectsInCamera[0].transform.position;
        }

        var bounds = new Bounds(objectsInCamera[0].transform.position, Vector3.zero);
        for (int i = 0; i < objectsInCamera.Count; i++)
        {
            bounds.Encapsulate(objectsInCamera[i].transform.position);
        }

        return bounds.center;
    }

    /// <summary>
    /// Draws lines around the bounding box that surround objects within the camera bounds
    /// </summary>
    /// <param name="b"></param>
    /// <param name="delay"></param>
    void DrawBounds(Bounds b, float delay = 0)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, Color.blue, delay);
        Debug.DrawLine(p2, p3, Color.red, delay);
        Debug.DrawLine(p3, p4, Color.yellow, delay);
        Debug.DrawLine(p4, p1, Color.magenta, delay);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, Color.blue, delay);
        Debug.DrawLine(p6, p7, Color.red, delay);
        Debug.DrawLine(p7, p8, Color.yellow, delay);
        Debug.DrawLine(p8, p5, Color.magenta, delay);

        // sides
        Debug.DrawLine(p1, p5, Color.white, delay);
        Debug.DrawLine(p2, p6, Color.gray, delay);
        Debug.DrawLine(p3, p7, Color.green, delay);
        Debug.DrawLine(p4, p8, Color.cyan, delay);
    }
}
