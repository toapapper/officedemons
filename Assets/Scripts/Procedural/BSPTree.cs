using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Code by: Tim & Kristian
/// <para>
/// A level making algoritm
/// </para>
/// </summary>
///
// Last Edited: 22/10/2021
public class BSPTree : MonoBehaviour
{

    public static BSPTree Instance { get; private set; }


    private Node root;
    private Node lastRoot;
    private GenerateTerrain generateTerrain;
    private FitnessFunction fitnessFunction;
    /// <summary>A GameObject that holds the script for NavMeshBaking</summary>
    [SerializeField]
    private GameObject level;
    private List<Node> nodes;
    [SerializeField]
    private int generations = 2;
    private Vector2 size, oldSize;

    [SerializeField] private Vector2 widthLimits = new Vector2(800,1000);
    [SerializeField] private Vector2 heightLimits = new Vector2(800,1000);
    /// <summary>A counter which gets bigger after each generation</summary>
    private int missfallMultiplier;
    /// <summary> If missfallMultiplier happens to land on missfallTop then no more children for the node </summary>
    [SerializeField]
    private int missfallTop = 6;
    //Rename this, pretty sure this is the total size
    private Vector2 lastSize;

    public Vector2 LastSize
    {
        get { return lastSize; }
        set { lastSize = value; }
    }

    private int fitnessGoal = 50;
    /// <summary> How many retires the obstacles should have </summary>
    private int bspRemakeTries = 100;
    private int nextDirection;
    private int lastDirection;

    [SerializeField]
    private bool startWithMap;

    [SerializeField]
    private int multipleRooms = 20;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        size.x = Random.Range((int)widthLimits.x, (int)widthLimits.y);
        size.y = Random.Range((int)heightLimits.x, (int)heightLimits.y);
        generateTerrain = GetComponent<GenerateTerrain>();
        fitnessFunction = GetComponent<FitnessFunction>();
        //if (startWithMap)
        //{
        //    for (int i = 0; i < startingRooms; i++)
        //    {
        //        MakeBSP();
        //    }
        //}
    }
    /// <summary>
    /// Make 100 rooms
    /// </summary>
    public void MakeMultipleRooms()
    {
        for (int i = 0; i < multipleRooms; i++)
        {
            MakeBSP();
        }
    }

    /// <summary>
    /// Start the algoritm
    /// </summary>
    public void MakeBSP()
    {
        fitnessFunction.RoomUpdate();
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
        if (FitnessFunction.currentRoom == Rooms.Encounter)
        {
            root.Encounter = true;
        }
        generateTerrain.GenerateGround(root);
        SearchObstaclesFitness(bspRemakeTries);

        size = fitnessFunction.NextRoomFitness(widthLimits, heightLimits,size,oldSize,lastSize,generations);

        lastDirection = nextDirection;
        GO();
        //generateTerrain.GenerateFullWalls(root, nextDirection,lastDirection, size,lastRoot.size,new Vector2(1,1), heightLimits.y);
        generateTerrain.GenerateFullBuildings(root, nextDirection, lastDirection, size, lastRoot.size);
        //Bakes a navMesh on the generated level
         //level.GetComponent<NavigationBaker>().BakeNavMesh();
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
            if (!fitnessFunction.FitnessFuntion(nodes, root))
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
            Debug.Log("last resort");
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
    private void Split(Node node)
    {
        if (node.generation >= generations)
        {
            return;
        }
        if (node.children[0] != null || node.children[1] != null)
        {
            return;
        }
        if (node.generation == 0)
        {

        }
        else if(node.parent.generation > node.generation)
        {
            return;
        }

        node.leaf = false;

        Node node1, node2;

        int split;
        int missfall = Random.Range(missfallMultiplier, missfallTop);
        //if (missfall == missfallMultiplier && node.generation > generations / 2)
        //{
        //    return;
        //}
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
    /// Walk in either up och right direction
    /// </summary>
    private void GO()
    {
        //if (fitnessFunction.TimeToTurn())
        //{
        //    GoRight();
        //}
        //else
        //{
        //    GoUp();
        //}
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
