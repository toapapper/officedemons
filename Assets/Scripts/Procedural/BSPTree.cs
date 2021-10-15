using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Code by: Tim & Kristian
/// </summary>
public class BSPTree : MonoBehaviour
{
    /// <summary>
    /// The Root is the whole room
    /// </summary>
    Node root;
    /// <summary>
    /// Last Room
    /// </summary>
    Node lastRoot;
    public GenerateTerrain generateTerrain;
    public FitnessFunction fitnessFunction;
    /// <summary>
    /// A list of all the nodes in a room
    /// </summary>
    public List<Node> nodes;
    /// <summary>
    /// How many generations of children a room should have
    /// </summary>
    public int generations = 3;
    /// <summary>
    /// Size for the Root and oldSize for the previous root size
    /// </summary>
    public Vector2 size, oldSize;
    /// <summary>
    /// The width space of a level
    /// </summary>
    public Vector2 widthLimits = new Vector2(800,1000);
    /// <summary>
    /// The height space of a level
    /// </summary>
    public Vector2 heightLimits = new Vector2(800,1000);
    /// <summary>
    /// A counter which gets bigger after each generation
    /// </summary>
    int missfallMultiplier;
    /// <summary>
    /// If missfallMultiplier happens to land on missfallTop then no more children for the node
    /// </summary>
    public int missfallTop = 6;
    /// <summary>
    /// The size of the last Root
    /// </summary>
    Vector2 lastSize;
    /// <summary>
    /// What is the goal for the obstacles made
    /// </summary>
    public int fitnessGoal = 50;
    /// <summary>
    /// How many retires the obstacles should have
    /// </summary>
    public int bspRemakeTries = 100;
    /// <summary>
    /// Where the next room will be placed
    /// </summary>
    int nextDirection;
    /// <summary>
    /// Where the last room was placed
    /// </summary>
    int lastDirection;


    private void Start()
    {
        size.x = Random.Range((int)widthLimits.x, (int)widthLimits.y);
        size.y = Random.Range((int)heightLimits.x, (int)heightLimits.y);
        generateTerrain = GetComponent<GenerateTerrain>();
        fitnessFunction = GetComponent<FitnessFunction>();
    }
    /// <summary>
    /// Make 100 rooms
    /// </summary>
    public void Make100BSP()
    {
        for (int i = 0; i < 100; i++)
        {
            MakeBSP();
        }
    }
    /// <summary>
    /// Start the algoritm
    /// </summary>
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
        //Happens only for the first one
        else
        {
            lastRoot = new Node(Vector2.zero, Vector2.zero);
        }

        root = new Node(size,lastSize);
        generateTerrain.GenerateGround(root);

        SearchObstaclesFitness(bspRemakeTries);

        size = fitnessFunction.NextRoomFitness(widthLimits, heightLimits, size,generations);

        lastDirection = nextDirection;
        GO();
        generateTerrain.GenerateFullWalls(root, nextDirection,lastDirection, size,lastRoot.size,new Vector2(1,1), heightLimits.y);
    }

    /// <summary>
    /// Search for a suitable interior to the room
    /// </summary>
    /// <param name="totalTries">How many tries are allowed? If none are acceptable, use the best one</param>
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
        {
            fitnessFunction.UseBestVariant();
        }

    }
    /// <summary>
    /// Create a room
    /// </summary>
    /// <param name="node">Chosen node to create a room out off</param>
    public void BSP(Node node)
    {
        nodes.Add(node);

        Split(node);
        missfallMultiplier++;
    }


    /// <summary>
    /// Split the node. Will contine to split until the asked generations are made
    /// </summary>
    /// <param name="node">The node which will be split</param>
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


            node1 = new Node(node, new Vector2(node.size.x, splitPoint), new Vector2(node.origin.x,node.origin.y + node.size.y - splitPoint), node.generation + 1);
            node.children[0] = node1;
            node2 = new Node(node, new Vector2(node.size.x, node.size.y - splitPoint), new Vector2(node.origin.x, node.origin.y), node.generation + 1);
            node.children[1] = node2;

            BSP(node1);
            BSP(node2);
        }
    }

    /// <summary>
    /// Create a buffer for a node
    /// </summary>
    /// <param name="value">the value which will be changed</param>
    /// <param name="node"> which node needs a buffer</param>
    /// <param name="vertical"> true for vertical | false for horizontal</param>
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
    /// Returns a random number between 0 and 1
    /// 0 = foward
    /// 1 = up
    /// </summary>
    private int GetDirection()
    {
        int d = Random.Range(0, 6);

        return d;
    }


    /// <summary>
    /// Walk in either up och right direction
    /// </summary>
    private void GO()
    {
        int direction = GetDirection();

        if (direction == 0)
            GoRight();
        else
            GoUp();

    }
    /// <summary>
    /// Next Direction is right <br/>
    /// Add to the overall size of the map
    /// </summary>
    private void GoRight()
    {
        nextDirection = 0;
        lastSize.x += oldSize.x;
    }
    /// <summary>
    /// Next Direction is up <br/>
    /// Add to the overall size of the map
    /// </summary>
    private void GoUp()
    {
        nextDirection = 1;
        lastSize.y += oldSize.y;
    }
}
