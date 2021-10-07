using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject level;
    public Material mat;
    public List<GameObject> cubes = new List<GameObject>();
    public TransformMesh transformMesh;
    int yvalue;

    private void Start()
    {
        transformMesh = GetComponent<TransformMesh>();
        //lbNodes = new List<Node>();
    }

    /// <summary>
    /// Instansiates a ground model from a quad prefab.
    /// </summary>
    /// <param name="node"></param>
    public void GenerateGround(Node node)
    {
        GameObject quad = Instantiate(groundPrefab, new Vector3(node.position.x, 0, node.position.y), Quaternion.identity);
        quad.transform.localScale = new Vector3(node.size.x, node.size.y, 1);
        quad.transform.rotation = Quaternion.Euler(90, 0, 0);
        quad.gameObject.name = "Ground";
        node.gameObject = quad;
        //quad.isStatic = true;
        cubes.Add(quad);
        quad.GetComponent<MeshRenderer>().material = mat;
        quad.transform.parent = level.transform;
    }

    //Optimize
    //nextDirection == 0 : next direction is right
    //nextDirection == 1 : next direction is up
    //Clean up needed.
    public void GenerateFullWalls(Node node,int nextDirection, int lastDirection,Vector2 nextSize,Vector2 lastSize,Vector2 wallSize, float howTall)
    {
        if (nextDirection == 0)
        {
            GameObject upWall = Instantiate(groundPrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y), Quaternion.Euler(new Vector3(0, 0, 0)));
            upWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
            upWall.transform.parent = level.transform;
            upWall.name = "upWall";

            if (lastDirection == 0)
            {
                GameObject downWall = Instantiate(groundPrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y - node.size.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = level.transform;
                downWall.name = "downWall";
            }
            else if (lastDirection == 1)
            {
                GameObject leftWall = Instantiate(groundPrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y - node.size.y / 2), Quaternion.Euler(new Vector3(0,-90,0)));
                leftWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
                leftWall.transform.parent = level.transform;
                leftWall.name = "leftWall";
            }

            if (node.size.y > nextSize.y)
            {
                GameObject rightWall = Instantiate(groundPrefab, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y - nextSize.y - ((node.size.y - nextSize.y) / 2)), Quaternion.Euler(new Vector3(0, 90, 0)));
                rightWall.transform.localScale = new Vector3(node.size.y - nextSize.y, howTall, wallSize.y);
                rightWall.transform.parent = level.transform;
                rightWall.name = "rightWall1";
            }
        }


        else if (nextDirection == 1)
        {
            GameObject rightWall = Instantiate(groundPrefab, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y - node.size.y / 2), Quaternion.Euler(new Vector3(0, 90, 0)));
            rightWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
            rightWall.transform.parent = level.transform;
            rightWall.name = "rightWall2";

            if (lastDirection == 1)
            {
                GameObject leftWall = Instantiate(groundPrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y - node.size.y / 2), Quaternion.Euler(new Vector3(0, -90, 0)));
                leftWall.transform.localScale = new Vector3(node.size.y, howTall, wallSize.y);
                leftWall.transform.parent = level.transform;
                leftWall.name = "leftWall";
            }
            else if (lastDirection == 0)
            {
                GameObject downWall = Instantiate(groundPrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2 /*+ howTall/100*/, node.origin.y - node.size.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = level.transform;
                downWall.name = "downWall";
            }

            if (node.size.x > nextSize.x)
            {
                GameObject upWallsmall = Instantiate(groundPrefab, new Vector3(node.origin.x + nextSize.x + ((node.size.x - nextSize.x)/2), howTall / 2, node.origin.y), Quaternion.identity);
                upWallsmall.transform.localScale = new Vector3(node.size.x - nextSize.x, howTall, wallSize.y);
                upWallsmall.transform.parent = level.transform;
                upWallsmall.name = "upWallsmall";
            }
        }


        if (lastDirection == 0)
        {
            if (node.size.y > lastSize.y)
            {
                GameObject leftWallsmall = Instantiate(groundPrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y - lastSize.y - ((node.size.y - lastSize.y) / 2)), Quaternion.Euler(new Vector3(0, -90, 0)));
                leftWallsmall.transform.localScale = new Vector3(node.size.y - lastSize.y, howTall,wallSize.y);
                leftWallsmall.transform.parent = level.transform;
                leftWallsmall.name = "leftWallsmall";

            }
        }
        if (lastDirection == 1)
        {
            if (node.size.x > lastSize.x)
            {
                GameObject downWallsmall = Instantiate(groundPrefab, new Vector3(node.origin.x + lastSize.x + ((node.size.x - lastSize.x) / 2), howTall / 2 /*+ howTall/100*/, node.origin.y - node.size.y), Quaternion.Euler(new Vector3(0, 180, 0)));
                downWallsmall.transform.localScale = new Vector3(node.size.x - lastSize.x, howTall, wallSize.y);
                downWallsmall.transform.parent = level.transform;
                downWallsmall.name = "downWallsmall";

            }
        }
    }

    //public void GenerateObstacles(Node node, Node root, float heightLimit)
    //{
    //    float x, y;
    //    BufferMaker(out x, out y, node);

    //    yvalue = (int)heightLimit / 20;
    //    GameObject cube = Instantiate(cubePrefab, new Vector3(node.position.x, yvalue, node.position.y), Quaternion.identity);
    //    cube.transform.localScale = new Vector3(x, yvalue * 2, y);
    //    node.gameObject = cube;
    //    cube.transform.parent = root.gameObject.transform;
    //    cube.isStatic = true;
    //    cubes.Add(cube);
    //    transformMesh.GetTexture(cube);
    //}
}
