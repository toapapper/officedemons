using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject groundPrefab;
    public Material mat;
    public List<GameObject> cubes = new List<GameObject>();
    public TransformMesh transformMesh;
    int yvalue;
    private void Start()
    {
        transformMesh = GetComponent<TransformMesh>();
    }
    private void BufferMaker(out float x, out float y, Node node)
    {
        float bufferX = node.size.x / 4;
        float bufferY = node.size.y / 4;
        x = Random.Range(bufferX, node.size.x - bufferX);
        y = Random.Range(bufferY, node.size.y - bufferY);

    }

    private bool TooCloseCheck(Node node, float distanceMultiplier, Node root, int width, int height)
    {
        if (Mathf.Abs(node.origin.x - root.origin.x) < width / distanceMultiplier)
            return true;

        if (Mathf.Abs(node.origin.y - root.origin.y) < height / distanceMultiplier)
            return true;

        if (Mathf.Abs((node.origin.y - node.size.y) - (root.origin.y - root.size.y)) < height / distanceMultiplier)
            return true;

        return false;
    }

    /// <summary>
    /// Instansiates a ground model from a quad prefab.
    /// </summary>
    /// <param name="node"></param>
    public void GenerateGround(Node node)
    {
        GameObject quad = Instantiate(groundPrefab, new Vector3(node.position.x, 1, node.position.y), Quaternion.identity);
        quad.transform.localScale = new Vector3(node.size.x, node.size.y, 1);
        quad.transform.rotation = Quaternion.Euler(90, 0, 0);
        quad.gameObject.name = "Ground";
        node.gameObject = quad;
        quad.isStatic = true;
        cubes.Add(quad);
        quad.GetComponent<MeshRenderer>().material = mat;
    }

    //Optimize
    //nextDirection == 0 : next direction is right
    //nextDirection == 1 : next direction is up
    //Clean up needed.
    public void GenerateFullWalls(Node node,int nextDirection, int lastDirection,Vector2 nextSize,Vector2 lastSize,Vector2 wallSize, float howTall)
    {
        if (nextDirection == 0)
        {
            GameObject upWall = Instantiate(cubePrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y), Quaternion.identity);
            upWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
            upWall.transform.parent = node.gameObject.transform;

            if (lastDirection == 0)
            {
                GameObject downWall = Instantiate(cubePrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2, node.origin.y - node.size.y), Quaternion.identity);
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = node.gameObject.transform;
            }
            else if (lastDirection == 1)
            {
                GameObject leftWall = Instantiate(cubePrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y - node.size.y / 2), Quaternion.identity);
                leftWall.transform.localScale = new Vector3(wallSize.x, howTall, node.size.y);
                leftWall.transform.parent = node.gameObject.transform;
            }

            if (node.size.y > nextSize.y)
            {
                GameObject rightWall = Instantiate(cubePrefab, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y - nextSize.y - ((node.size.y - nextSize.y) / 2)), Quaternion.identity);
                rightWall.transform.localScale = new Vector3(wallSize.x, howTall, node.size.y - nextSize.y);
                rightWall.transform.parent = node.gameObject.transform;
            }
        }


        else if (nextDirection == 1)
        {
            GameObject rightWall = Instantiate(cubePrefab, new Vector3(node.origin.x + node.size.x, howTall / 2, node.origin.y - node.size.y / 2), Quaternion.identity);
            rightWall.transform.localScale = new Vector3(wallSize.x, howTall, node.size.y);
            rightWall.transform.parent = node.gameObject.transform;
            if (lastDirection == 1)
            {
                GameObject leftWall = Instantiate(cubePrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y - node.size.y / 2), Quaternion.identity);
                leftWall.transform.localScale = new Vector3(wallSize.x, howTall, node.size.y);
                leftWall.transform.parent = node.gameObject.transform;
            }
            else if (lastDirection == 0)
            {
                GameObject downWall = Instantiate(cubePrefab, new Vector3(node.origin.x + node.size.x / 2, howTall / 2 /*+ howTall/100*/, node.origin.y - node.size.y), Quaternion.identity);
                downWall.transform.localScale = new Vector3(node.size.x, howTall, wallSize.y);
                downWall.transform.parent = node.gameObject.transform;
            }

            if (node.size.x > nextSize.x)
            {
                GameObject upWallsmall = Instantiate(cubePrefab, new Vector3(node.origin.x + nextSize.x + ((node.size.x - nextSize.x)/2), howTall / 2, node.origin.y), Quaternion.identity);
                upWallsmall.transform.localScale = new Vector3(node.size.x - nextSize.x, howTall, wallSize.y);
                upWallsmall.transform.parent = node.gameObject.transform;
            }
        }


        if (lastDirection == 0)
        {
            if (node.size.y > lastSize.y)
            {
                GameObject leftWallsmall = Instantiate(cubePrefab, new Vector3(node.origin.x, howTall / 2, node.origin.y - lastSize.y - ((node.size.y - lastSize.y) / 2)), Quaternion.identity);
                leftWallsmall.transform.localScale = new Vector3(wallSize.x, howTall, node.size.y - lastSize.y);
                leftWallsmall.transform.parent = node.gameObject.transform;
            }
        }
        else if (lastDirection == 1)
        {
            if (node.size.x > lastSize.x)
            {
                GameObject downWallsmall = Instantiate(cubePrefab, new Vector3(node.origin.x + lastSize.x + ((node.size.x - lastSize.x) / 2), howTall / 2 /*+ howTall/100*/, node.origin.y - node.size.y), Quaternion.identity);
                downWallsmall.transform.localScale = new Vector3(node.size.x - lastSize.x, howTall, wallSize.y);
                downWallsmall.transform.parent = node.gameObject.transform;
            }

        }


    }

    public void GenerateObstacles(Node node, Node root, int width, int height, float heightLimit)
    {
        float x, y;
        BufferMaker(out x, out y, node);
        //If they become too small use this
        //if (node.size.x * node.size.y < width * height / 200)
        //    return;

        if (TooCloseCheck(node, 20, root, width, height))
            return;

        yvalue =(int)heightLimit / 20;
        GameObject cube = Instantiate(cubePrefab, new Vector3(node.position.x, yvalue, node.position.y), Quaternion.identity);
        cube.transform.localScale = new Vector3(x, yvalue * 2, y);
        node.gameObject = cube;
        cube.transform.parent = root.gameObject.transform;
        cube.isStatic = true;
        cubes.Add(cube);
        transformMesh.GetTexture(cube);
    }
}
