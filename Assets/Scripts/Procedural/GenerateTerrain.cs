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

// Last Edited: 21/10/2021


public class GenerateTerrain : MonoBehaviour
{
    /// <summary>Material used to create walls</summary>
    [SerializeField]
    private GameObject quadPrefabWalls;

    [SerializeField]
    private List<GameObject> housesPrefabs = new List<GameObject>();

    /// <summary>Material used to create walkable floor</summary>
    [SerializeField]
    private GameObject quadPrefabWalkable;
    /// <summary>Parent to everything made </summary>
    [SerializeField]
    private GameObject level;
    /// <summary>What material the quadPrefab should have</summary>
    [SerializeField]
    private Material mat;
    /// <summary> A list of all floors </summary>
    private List<GameObject> cubes = new List<GameObject>();

    [SerializeField]
    private float minSizeHouses = 5;
    /// <summary>
    /// Instansiates a ground model from a quad prefab.
    /// </summary>
    /// <param name="node">Root node goes here</param>
    public void GenerateGround(Node node)
    {
        GameObject quad = Instantiate(quadPrefabWalkable, new Vector3(node.position.x, 0, node.position.y), Quaternion.identity);
        quad.transform.localScale = new Vector3(node.size.x, 0, node.size.y);
        //quad.transform.rotation = Quaternion.Euler(90, 0, 0);
        quad.gameObject.name = "Ground";
        node.gameObject = quad;
        //quad.isStatic = true;
        cubes.Add(quad);
        quad.GetComponent<MeshRenderer>().material = mat;
        quad.tag = "Walkable";
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
            GameObject upWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y + node.size.y), Quaternion.Euler(new Vector3(0, 0, 0)));
            upWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
            upWall.transform.parent = level.transform;
            upWall.name = "upWall1";

            if (lastDirection == 0)
            {
                GameObject downWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = level.transform;
                downWall.name = "downWall1";
            }
            else if (lastDirection == 1)
            {
                GameObject leftWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0,-90,0)));
                leftWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
                leftWall.transform.parent = level.transform;
                leftWall.name = "leftWall1";
            }
            //next room has a smaller size so make a wall to cover it up
            if (node.size.y > nextSize.y)
            {
                GameObject rightWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y + nextSize.y + ((node.size.y - nextSize.y) / 2)), Quaternion.Euler(new Vector3(0, 90, 0)));
                rightWallsmall.transform.localScale = new Vector3(node.size.y - nextSize.y, howTall, wallSize.y);
                rightWallsmall.transform.parent = level.transform;
                rightWallsmall.name = "rightWallsmall";
            }
        }


        else if (nextDirection == 1)
        {

            //GameObject rightWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0, 90, 0)));
            GameObject rightWall = Instantiate(housesPrefabs[0], new Vector3(node.origin.x + node.size.x + GetXScale(node,nextSize, lastSize, minSizeHouses), node.size.y / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(-90, 0, 0)));
            rightWall.transform.localScale = new Vector3(GetXScale(node, nextSize, lastSize, minSizeHouses), node.size.y / 2, node.size.y / 2);
            rightWall.transform.parent = level.transform;
            rightWall.name = "rightWall2";

            if (lastDirection == 1)
            {
                GameObject leftWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0, -90, 0)));
                leftWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
                leftWall.transform.parent = level.transform;
                leftWall.name = "leftWall2";
            }
            else if (lastDirection == 0)
            {
                GameObject downWall = Instantiate(housesPrefabs[0], new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y), Quaternion.Euler(new Vector3(-90, 90, 0)));
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = level.transform;
                downWall.name = "downWall2";
            }
            //next room has a smaller size so make a wall to cover it up
            if (node.size.x > nextSize.x)
            {
                //GameObject upWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + nextSize.x + ((node.size.x - nextSize.x)/2), howTall / 2, node.origin.y + node.size.y), Quaternion.identity);
                //upWallsmall.transform.localScale = new Vector3(node.size.x - nextSize.x, howTall, wallSize.y);
                //upWallsmall.transform.parent = level.transform;
                //upWallsmall.name = "upWallsmall";
            }
        }


        if (lastDirection == 0)
        {
            //next room has a smaller size so make a wall to cover it up
            if (node.size.y > lastSize.y)
            {
                GameObject leftWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x, howTall / 2, node.origin.y + lastSize.y + ((node.size.y - lastSize.y) / 2)), Quaternion.Euler(new Vector3(0, -90, 0)));
                leftWallsmall.transform.localScale = new Vector3(node.size.y - lastSize.y, howTall,wallSize.y);
                leftWallsmall.transform.parent = level.transform;
                leftWallsmall.name = "leftWallsmall";

            }
        }
        if (lastDirection == 1)
        {
            //next room has a smaller size so make a wall to cover it up
            if (node.size.x > lastSize.x)
            {
                //GameObject downWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + lastSize.x + ((node.size.x - lastSize.x) / 2), howTall / 2 /*+ howTall/100*/, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                //downWallsmall.transform.localScale = new Vector3(node.size.x - lastSize.x, howTall, wallSize.y);
                //downWallsmall.transform.parent = level.transform;
                //downWallsmall.name = "downWallsmall";

            }
        }
    }


    private float GetXScale(Node node, Vector2 nextSize, Vector2 lastSize, float minSize)
    {
        float x = 0;
        if (nextSize.x > lastSize.x)
        {
            if (node.size.x < nextSize.x)
            {
                x = nextSize.x - node.size.x;
                x = x / 2;
            }
        }
        else
        {
            if (node.size.x < lastSize.x)
            {
                x = lastSize.x - node.size.x;
                x = x / 2;
            }
        }
        if (x < minSize)
        {
            x = minSize;
        }
        return x;
    }
}
