using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPTree : MonoBehaviour
{
    Node root;
    public GenerateTerrain generateTerrain;
    //public GameObject cubePrefab;
    //public GameObject groundPrefab;
    public List<Node> nodes;
    //public List<GameObject> cubes = new List<GameObject>();
    public int generations = 3;
    int width, oldWidth;
    int height, oldHeight;
    public Vector2 widthLimits = new Vector2(800,1000);
    public Vector2 heightLimits = new Vector2(800,1000);
    int missfallMultiplier;
    public int missfallTop = 6;
    Vector2 lastSize;
    int lastDirection;

    private void Start()
    {
        width = Random.Range((int)widthLimits.x, (int)widthLimits.y);
        height = Random.Range((int)heightLimits.x, (int)heightLimits.y);
        generateTerrain = GetComponent<GenerateTerrain>();
    }

    public void MakeBSP()
    {
        oldWidth = width;
        oldHeight = height;
        missfallMultiplier = 0;
        nodes = new List<Node>();
        root = new Node(new Vector2(width, height),lastSize);
        generateTerrain.GenerateGround(root);
        BSP(root);
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].leaf)
                generateTerrain.GenerateObstacles(nodes[i], root, width, height);
        }
        Debug.Log(nodes.Count);

        width = Random.Range((int)widthLimits.x,(int)widthLimits.y);
        height = Random.Range((int)heightLimits.x, (int)heightLimits.y);
        GO();
    }

    public void BSP(Node node)
    {
        nodes.Add(node);

        Split(node);
        missfallMultiplier++;
    }

    /// <summary>
    /// Evaluates the BSP tree after a set of desirable traits that correlates to a fitness value.
    /// </summary>
    /// <param name="nodes"></param>
    public void FitnessFuntion(List<Node> nodes, int desiredFitnessValue)
    {
        int fitnessValue = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            //Evaluate fitness.
        }

        if(fitnessValue == desiredFitnessValue)
        {
            return;
        }
        else
        {
            MakeBSP();
            FitnessFuntion(nodes, desiredFitnessValue);
        }
    }

    public void Split(Node node)
    {
        if (node.generation >= generations)
            return;
        if (node.children[0] != null || node.children[1] != null)
            return;
        if (node.generation == 0)
        {

        }
        else if(node.parent.generation > node.generation)
            return;

        node.leaf = false;

        Node node1, node2;

        int split;
        int missfall = Random.Range(missfallMultiplier, missfallTop);
        if (missfall == missfallMultiplier && node.generation > 2)
            return;
        split = Random.Range(0, 2);
        float buffer = 0;

        if (split == 0)
        {
            //Split vertically
            BufferMaker(out buffer,node,true);
            float splitPoint = Random.Range(buffer,node.size.x - buffer);

            node1 = new Node(node, new Vector2(splitPoint, node.size.y), node.origin, node.generation + 1);
            node.children[0] = node1;
            node2 = new Node(node, new Vector2(node.size.x - splitPoint, node.size.y), new Vector2(node.origin.x + splitPoint, node.origin.y), node.generation + 1);
            node.children[1] = node2;

            BSP(node1);
            BSP(node2);
        }
        else
        {
            //Split horizontal

            BufferMaker(out buffer, node,false);
            float splitPoint = Random.Range(buffer, node.size.y - buffer);


            node1 = new Node(node, new Vector2(node.size.x, splitPoint), node.origin, node.generation + 1);
            node.children[0] = node1;
            node2 = new Node(node, new Vector2(node.size.x, node.size.y - splitPoint), new Vector2(node.origin.x, node.origin.y - splitPoint), node.generation + 1);
            node.children[1] = node2;

            BSP(node1);
            BSP(node2);
        }
    }

    /// <summary>
    /// true for vertical
    /// false for horizontal
    /// </summary>
    /// <param name="value"></param>
    /// <param name="node"></param>
    /// <param name="vertical"></param>
    private void BufferMaker(out float value, Node node, bool vertical)
    {
        float buffer;

        if (vertical)
        {
            buffer = node.size.x / 4;
            value = Random.Range(buffer, node.size.x - buffer);

        }
        else
        {
            buffer = node.size.y / 4;
            value = Random.Range(buffer, node.size.y - buffer);
        }
    }


    /// <summary>
    /// Get a random number between 0 and 2
    /// 0 = foward
    /// 1 = up
    /// 2 = down
    /// </summary>
    /// <returns></returns>
    private int GetDirection()
    {
        int d;
        while (true)
        {
            d = Random.Range(0, 2);
            if (d == 0)
                break;
            else if (d == 1 && lastDirection != 2)
                break;
            else if (d == 2 && lastDirection != 1)
                break;

        }
        return d;
    }

    private void GO()
    {
        int direction = GetDirection();

        if (direction == 0)
            GoRight();
        else if (direction == 1)
            GoUp();
    }
    private void GoRight()
    {
        lastDirection = 0;
        lastSize.x += oldWidth;
    }
    private void GoUp()
    {
        lastDirection = 1;
        lastSize.y += height;
    }
}
