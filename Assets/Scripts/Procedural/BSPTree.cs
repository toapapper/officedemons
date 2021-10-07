using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Code by: Tim & Kristian
/// </summary>
public class BSPTree : MonoBehaviour
{
    Node root;
    Node lastRoot;
    public GenerateTerrain generateTerrain;
    public FitnessFunction fitnessFunction;

    public List<Node> nodes;

    public int generations = 3;
    public Vector2 size, oldSize;
    public Vector2 widthLimits = new Vector2(800,1000);
    public Vector2 heightLimits = new Vector2(800,1000);
    int missfallMultiplier;
    public int missfallTop = 6;
    Vector2 lastSize;
    public int fitnessGoal = 50;
    public int bspRemakeTries = 100;
    //Maybe throw them into node?
    int nextDirection;
    int lastDirection;

    private void Start()
    {
        size.x = Random.Range((int)widthLimits.x, (int)widthLimits.y);
        size.y = Random.Range((int)heightLimits.x, (int)heightLimits.y);
        generateTerrain = GetComponent<GenerateTerrain>();
        fitnessFunction = GetComponent<FitnessFunction>();
    }

    public void Make100BSP()
    {
        for (int i = 0; i < 100; i++)
        {
            MakeBSP();
        }
    }


    public void MakeBSP()
    {
        missfallMultiplier = 0;
        nodes = new List<Node>();
        oldSize.x = size.x;
        oldSize.y = size.y;
        if (root != null)
        {
            lastRoot = root;
        } 
        else
        {
            lastRoot = new Node(Vector2.zero, Vector2.zero);
        }

        root = new Node(size,lastSize);
        generateTerrain.GenerateGround(root);

        SearchObstaclesFitness(bspRemakeTries);
        
        size = fitnessFunction.NextRoomFitness(widthLimits, heightLimits, size);

        lastDirection = nextDirection;
        GO();
        generateTerrain.GenerateFullWalls(root, nextDirection,lastDirection, size,lastRoot.size,new Vector2(1,1), heightLimits.y);
    }

    private void SearchObstaclesFitness(int totalTries)
    {
        bool foundSuitableObstacles = false;
        for (int i = 0; i < totalTries; i++)
        {
            BSP(root);

            //Search again
            if (!fitnessFunction.FitnessFuntion(nodes, root, foundSuitableObstacles, (int)heightLimits.y))
            {
                nodes = new List<Node>();
                root.children = new Node[2];
            }
            else
            {
                foundSuitableObstacles = true;
                break;
            }
        }
        if (!foundSuitableObstacles)
            fitnessFunction.UseBestVariant();
    }

    public void BSP(Node node)
    {
        nodes.Add(node);

        Split(node);
        missfallMultiplier++;
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
        if (missfall == missfallMultiplier && node.generation > generations/2)
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


            node1 = new Node(node, new Vector2(node.size.x, splitPoint), new Vector2(node.origin.x,node.origin.y + splitPoint), node.generation + 1);
            node.children[0] = node1;
            node2 = new Node(node, new Vector2(node.size.x, node.size.y - splitPoint), new Vector2(node.origin.x, node.origin.y), node.generation + 1);
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
    /// Get a random number between 0 and 1
    /// 0 = foward
    /// 1 = up
    /// </summary>
    /// <returns></returns>
    private int GetDirection()
    {
        int d = Random.Range(0, 6);

        return d;
    }

    private void GO()
    {
        int direction = GetDirection();

        if (direction == 0)
            GoRight();
        else
            GoUp();

    }

    private void GoRight()
    {
        nextDirection = 0;
        lastSize.x += oldSize.x;
    }

    private void GoUp()
    {
        nextDirection = 1;
        lastSize.y += oldSize.y;
    }
}