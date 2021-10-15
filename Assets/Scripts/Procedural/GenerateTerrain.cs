using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Generate Walls and floors 
/// <para>
/// Author: Tim & Krisitan
/// </para>
/// </summary>
/// 

// Last Edited: 13/10/2021


public class GenerateTerrain : MonoBehaviour
{
    /// <summary>
    /// Material used to create terrain(Walls and floor)
    /// </summary>
    public GameObject quadPrefab;
    /// <summary>
    /// Parent to everything made
    /// </summary>
    public GameObject level;
    /// <summary>
    /// What material the quadPrefab should have
    /// </summary>
    public Material mat;
    /// <summary>
    /// A list of all floors
    /// </summary>
    public List<GameObject> cubes = new List<GameObject>();


    /// <summary>
    /// Instansiates a ground model from a quad prefab.
    /// </summary>
    /// <param name="node">Root node goes here</param>
    public void GenerateGround(Node node)
    {
        GameObject quad = Instantiate(quadPrefab, new Vector3(node.position.x, 0, node.position.y), Quaternion.identity);
        quad.transform.localScale = new Vector3(node.size.x, node.size.y, 1);
        quad.transform.rotation = Quaternion.Euler(90, 0, 0);
        quad.gameObject.name = "Ground";
        node.gameObject = quad;
        //quad.isStatic = true;
        cubes.Add(quad);
        quad.GetComponent<MeshRenderer>().material = mat;
        quad.transform.parent = level.transform;
    }


    /// <summary>
    /// Create all the walls needed <br/>
    /// Some walls are not full walls depending on previous and upcoming rooms
    /// </summary>
    /// <param name="node">Root</param>
    /// <param name="nextDirection">Which direction the next root is going 0 = right 1 = up</param>
    /// <param name="lastDirection">Which direction the previous root is went 0 = right 1 = up</param>
    /// <param name="nextSize">The size of the upcoming Root</param>
    /// <param name="lastSize">The previous roots size</param>
    /// <param name="wallSize">The width of a wall</param>
    /// <param name="howTall">The height of a wall</param>
    public void GenerateFullWalls(Node node,int nextDirection, int lastDirection,Vector2 nextSize,Vector2 lastSize,Vector2 wallSize, float howTall)
    {
        if (nextDirection == 0)
        {
            GameObject upWall = Instantiate(quadPrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y + node.size.y), Quaternion.Euler(new Vector3(0, 0, 0)));
            upWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
            upWall.transform.parent = level.transform;
            upWall.name = "upWall1";

            if (lastDirection == 0)
            {
                GameObject downWall = Instantiate(quadPrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = level.transform;
                downWall.name = "downWall1";
            }
            else if (lastDirection == 1)
            {
                GameObject leftWall = Instantiate(quadPrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0,-90,0)));
                leftWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
                leftWall.transform.parent = level.transform;
                leftWall.name = "leftWall1";
            }

            if (node.size.y > nextSize.y)
            {
                GameObject rightWallsmall = Instantiate(quadPrefab, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y + nextSize.y + ((node.size.y - nextSize.y) / 2)), Quaternion.Euler(new Vector3(0, 90, 0)));
                rightWallsmall.transform.localScale = new Vector3(node.size.y - nextSize.y, howTall, wallSize.y);
                rightWallsmall.transform.parent = level.transform;
                rightWallsmall.name = "rightWallsmall";
            }
        }


        else if (nextDirection == 1)
        {
            GameObject rightWall = Instantiate(quadPrefab, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0, 90, 0)));
            rightWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
            rightWall.transform.parent = level.transform;
            rightWall.name = "rightWall2";

            if (lastDirection == 1)
            {
                GameObject leftWall = Instantiate(quadPrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0, -90, 0)));
                leftWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
                leftWall.transform.parent = level.transform;
                leftWall.name = "leftWall2";
            }
            else if (lastDirection == 0)
            {
                GameObject downWall = Instantiate(quadPrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2 /*+ howTall/100*/, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = level.transform;
                downWall.name = "downWall2";
            }

            if (node.size.x > nextSize.x)
            {
                GameObject upWallsmall = Instantiate(quadPrefab, new Vector3(node.origin.x + nextSize.x + ((node.size.x - nextSize.x)/2), howTall / 2, node.origin.y + node.size.y), Quaternion.identity);
                upWallsmall.transform.localScale = new Vector3(node.size.x - nextSize.x, howTall, wallSize.y);
                upWallsmall.transform.parent = level.transform;
                upWallsmall.name = "upWallsmall";
            }
        }


        if (lastDirection == 0)
        {
            if (node.size.y > lastSize.y)
            {
                GameObject leftWallsmall = Instantiate(quadPrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y + lastSize.y + ((node.size.y - lastSize.y) / 2)), Quaternion.Euler(new Vector3(0, -90, 0)));
                leftWallsmall.transform.localScale = new Vector3(node.size.y - lastSize.y, howTall,wallSize.y);
                leftWallsmall.transform.parent = level.transform;
                leftWallsmall.name = "leftWallsmall";

            }
        }
        if (lastDirection == 1)
        {
            if (node.size.x > lastSize.x)
            {
                GameObject downWallsmall = Instantiate(quadPrefab, new Vector3(node.origin.x + lastSize.x + ((node.size.x - lastSize.x) / 2), howTall / 2 /*+ howTall/100*/, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWallsmall.transform.localScale = new Vector3(node.size.x - lastSize.x, howTall, wallSize.y);
                downWallsmall.transform.parent = level.transform;
                downWallsmall.name = "downWallsmall";

            }
        }
    }
}
