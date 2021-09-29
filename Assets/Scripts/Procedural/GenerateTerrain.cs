using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateTerrain : MonoBehaviour
{
    [HideInInspector]
    public GameObject cubePrefab;
    public GameObject groundPrefab;
    public GameObject level;
    public Material mat;
    public List<GameObject> cubes = new List<GameObject>();
    public TransformMesh transformMesh;
    int yvalue;

    //lb = last best
    List<Node> lbNodes;
    Node lbRoot;
    int lbWidth;
    int lbHeight;
    int lbHeightLimit;
    float lbFitness;
    private void Start()
    {
        transformMesh = GetComponent<TransformMesh>();
        lbNodes = new List<Node>();
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

    private void BufferMaker(out float x, out float y, Node node)
    {
        float bufferX = node.size.x / 4;
        float bufferY = node.size.y / 4;
        x = Random.Range(bufferX, node.size.x - bufferX);
        y = Random.Range(bufferY, node.size.y - bufferY);

    }
    private float TooCloseBorders(Node node, float distanceMultiplier, Node root, int width, int height, float fitness, float penalty)
    {
        if (Mathf.Abs(node.origin.x - root.origin.x) < width / distanceMultiplier)
            fitness -= penalty;

        if (Mathf.Abs(node.origin.y - root.origin.y) < height / distanceMultiplier)
            fitness -= penalty;

        if (Mathf.Abs((node.origin.y - node.size.y) - (root.origin.y - root.size.y)) < height / distanceMultiplier)
            fitness -= penalty;

        return fitness;
    }

    private float TooCloseObstacles(List<Node> nodes, Node node, float distanceMultiplier, int width, int height, float fitness, float penalty)
    {

        for (int i = 0; i < nodes.Count; i++)
        {
            
        }





        return fitness;
    }
    public float FitnessCheck(Node node, Node root, int width, int height, float heightLimit, float fitness)
    {

        if (node.size.x * node.size.y < width * height / 200)
            return fitness -= 20;

        else if (node.size.x * node.size.y > width * height / 100)
            return fitness += 40;

        else
            fitness += 10;

        fitness = TooCloseBorders(node, 20, root, width, height, fitness, 400);

        return fitness;
    }




    public void GenerateObstacles(Node node, Node root, int width, int height, float heightLimit)
    {
        float x, y;
        BufferMaker(out x, out y, node);

        yvalue = (int)heightLimit / 20;
        GameObject cube = Instantiate(cubePrefab, new Vector3(node.position.x, yvalue, node.position.y), Quaternion.identity);
        cube.transform.localScale = new Vector3(x, yvalue * 2, y);
        node.gameObject = cube;
        cube.transform.parent = root.gameObject.transform;
        cube.isStatic = true;
        cubes.Add(cube);
        transformMesh.GetTexture(cube);
    }

    public bool SearchForObstacles(List<Node> nodes, Node root, int width, int height, int heightLimit, float fitnessGoal)
    {
        float fitness = 0;
        int obstacles = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].leaf)
            {
                fitness = FitnessCheck(nodes[i], root, width, height, heightLimit, fitness);
                obstacles++;
            }
            Debug.Log("fitness after check = " + fitness);
        }
        if (obstacles > 6)
        {
            fitness -= 400;
        }
        if (fitness >= fitnessGoal)
        {
            Debug.Log("Sucessesful fitness = " + fitness);
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].leaf)
                    GenerateObstacles(nodes[i], root, width, height, heightLimit);
            }
            ResetLastFitness();
            return true;
        }
        else
        {
            if (lbFitness < fitness)
            {
                lbFitness = fitness;
                lbNodes = nodes;
                lbRoot = root;
                lbWidth = width;
                lbHeight = height;
                lbHeightLimit = heightLimit;
            }
        }
        //Debug.Log(fitness);
        return false;
    }

    public void UseBestVariant()
    {
        for (int i = 0; i < lbNodes.Count; i++)
        {
            if (lbNodes[i].leaf)
                GenerateObstacles(lbNodes[i], lbRoot, lbWidth, lbHeight, lbHeightLimit);
        }
        ResetLastFitness();
        Debug.Log("Last resort was used");
    }



    public void ResetLastFitness()
    {
        lbFitness = 0;
        lbNodes = new List<Node>();
        lbRoot = new Node(Vector2.zero,Vector2.zero);
        lbWidth = 0;
        lbHeight = 0;
        lbHeightLimit = 0;
    }
}
