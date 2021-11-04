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
    private List<GameObject> smallWallPrefabs = new List<GameObject>();

    /// <summary>Material used to create walkable floor</summary>
    [SerializeField]
    private GameObject quadPrefabWalkable;
    /// <summary>Parent to everything made </summary>s
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

            GameObject rightWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0, 90, 0)));
            rightWall.transform.localScale = new Vector3(node.size.y, howTall, node.size.y);
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
                GameObject downWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = level.transform;
                downWall.name = "downWall2";
            }
            //next room has a smaller size so make a wall to cover it up
            if (node.size.x > nextSize.x)
            {
                GameObject upWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + nextSize.x + ((node.size.x - nextSize.x) / 2), howTall / 2, node.origin.y + node.size.y), Quaternion.identity);
                upWallsmall.transform.localScale = new Vector3(node.size.x - nextSize.x, howTall, wallSize.y);
                upWallsmall.transform.parent = level.transform;
                upWallsmall.name = "upWallsmall";
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
                GameObject downWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + lastSize.x + ((node.size.x - lastSize.x) / 2), howTall / 2 /*+ howTall/100*/, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWallsmall.transform.localScale = new Vector3(node.size.x - lastSize.x, howTall, wallSize.y);
                downWallsmall.transform.parent = level.transform;
                downWallsmall.name = "downWallsmall";

            }
        }
    }


    public void GenerateFullBuildings(Node node, int nextDirection, int lastDirection, Vector2 nextSize, Vector2 lastSize)
    {
        if (nextDirection == 0)
        {
            //GameObject upWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x / 2, node.origin.y + node.size.y), Quaternion.Euler(new Vector3(0, 0, 0)));
            //upWall.transform.localScale = new Vector3(node.size.x,);
            //upWall.transform.parent = level.transform;
            //upWall.name = "upWall1";

            if (lastDirection == 0)
            {
                //GameObject downWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x / 2, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                //downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                //downWall.transform.parent = level.transform;
                //downWall.name = "downWall1";
            }
            else if (lastDirection == 1)
            {
                //GameObject leftWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0, -90, 0)));
                //leftWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
                //leftWall.transform.parent = level.transform;
                //leftWall.name = "leftWall1";
            }
            //next room has a smaller size so make a wall to cover it up
            if (node.size.y > nextSize.y)
            {
                //GameObject rightWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y + nextSize.y + ((node.size.y - nextSize.y) / 2)), Quaternion.Euler(new Vector3(0, 90, 0)));
                //rightWallsmall.transform.localScale = new Vector3(node.size.y - nextSize.y, howTall, wallSize.y);
                //rightWallsmall.transform.parent = level.transform;
                //rightWallsmall.name = "rightWallsmall";
            }
        }


        else if (nextDirection == 1)
        {
            GameObject houseRight2 = SpawnItemsFromLibrary.Instance.FindClosestKey(node, ProceduralItemLibrary.Instance.housesDictonary);
            GameObject right2 = Instantiate(houseRight2, new Vector3(node.origin.x + node.size.x + (houseRight2.GetComponent<BoxCollider>().size.x * houseRight2.transform.localScale.x) / 2,
                (houseRight2.GetComponent<BoxCollider>().size.y * houseRight2.transform.localScale.z)/2,
                node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(-90, 0, 0)));

            //y positon change to house.transform.position.y when origin is fixed

            //Could've made a temp variable but then there would have to be alot of them for every different wall.
            right2.transform.localScale = new Vector3(right2.transform.localScale.x,
                right2.transform.localScale.y * GetRightSize(node, houseRight2),
                right2.transform.localScale.z);

            right2.transform.parent = level.transform;
            right2.name = "houseRight2";

            if (lastDirection == 1)
            {

                GameObject houseLeft2 = SpawnItemsFromLibrary.Instance.FindClosestKey(node, ProceduralItemLibrary.Instance.housesDictonary);
                GameObject left2 = Instantiate(houseLeft2, new Vector3(node.origin.x - (houseLeft2.GetComponent<BoxCollider>().size.x * houseLeft2.transform.localScale.x) / 2,
                    (houseLeft2.GetComponent<BoxCollider>().size.y * houseLeft2.transform.localScale.z) / 2,
                    node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(-90, 180, 0)));

                //y positon change to house.transform.position.y when origin is fixed

                //Could've made a temp variable but then there would have to be alot of them for every different wall.
                left2.transform.localScale = new Vector3(left2.transform.localScale.x,
                    left2.transform.localScale.y * GetRightSize(node, houseLeft2),
                    left2.transform.localScale.z);

                left2.transform.parent = level.transform;
                left2.name = "houseLeft2";



                //GameObject leftWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x, howTall / 2, node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(0, -90, 0)));
                //leftWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
                //leftWall.transform.parent = level.transform;
                //leftWall.name = "leftWall2";
            }
            else if (lastDirection == 0)
            {
                //GameObject downWall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                //downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                //downWall.transform.parent = level.transform;
                //downWall.name = "downWall2";
            }
            //next room has a smaller size so make a wall to cover it up
            if (node.size.x > nextSize.x)
            {
                //  TODO
                GameObject houseUpSmall = SpawnItemsFromLibrary.Instance.FindClosestKey(node, ProceduralItemLibrary.Instance.housesDictonary);
                GameObject upSmall = Instantiate(houseUpSmall, new Vector3(node.origin.x + node.size.x + (houseUpSmall.GetComponent<BoxCollider>().size.x * houseUpSmall.transform.localScale.x) / 2,
                    (houseUpSmall.GetComponent<BoxCollider>().size.y * houseUpSmall.transform.localScale.z) / 2,
                    node.origin.y + node.size.y / 2), Quaternion.Euler(new Vector3(-90, -90, 0)));

                //y positon change to house.transform.position.y when origin is fixed
                upSmall.transform.localScale = new Vector3(upSmall.transform.localScale.x,
                    upSmall.transform.localScale.y * GetRightSize(node, houseUpSmall),
                    upSmall.transform.localScale.z);
                upSmall.transform.parent = level.transform;
                upSmall.name = "houseUpSmall";


                //GameObject upWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x + nextSize.x + ((node.size.x - nextSize.x) / 2), howTall / 2, node.origin.y + node.size.y), Quaternion.identity);
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
                //GameObject leftWallsmall = Instantiate(quadPrefabWalls, new Vector3(node.origin.x, howTall / 2, node.origin.y + lastSize.y + ((node.size.y - lastSize.y) / 2)), Quaternion.Euler(new Vector3(0, -90, 0)));
                //leftWallsmall.transform.localScale = new Vector3(node.size.y - lastSize.y, howTall, wallSize.y);
                //leftWallsmall.transform.parent = level.transform;
                //leftWallsmall.name = "leftWallsmall";

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
    

    private float GetRightSize(Node node, GameObject house)
    {
        if (node.size.y < house.GetComponent<BoxCollider>().size.y * house.transform.localScale.y)
        {
            Debug.Log("house bigger " + "House: " + house.GetComponent<BoxCollider>().size.y * house.transform.localScale.y + " node:" + node.size.y);
            Debug.Log(node.size.y / (house.GetComponent<BoxCollider>().size.y * house.transform.localScale.y));
            return node.size.y / (house.GetComponent<BoxCollider>().size.y * house.transform.localScale.y);

        }
        else if (node.size.y > house.GetComponent<BoxCollider>().size.y * house.transform.localScale.y)
        {
            return node.size.y / (house.GetComponent<BoxCollider>().size.y * house.transform.localScale.y);
        }
        return 1;
    }
}
