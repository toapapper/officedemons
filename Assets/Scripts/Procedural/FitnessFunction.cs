using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>
/// Evaluates and calculates fitness for different rooms in the level.
/// </para>
///  <para>
///  Author: Tim & Kristian
/// </para>
/// </summary>

// Last Edited: 23-10-2021
//TODO: tog bort fitness tempor�rt och allt den kollar p� �r atm desiredobjects aka s� h�r m�nga vapen/fiender. L�gg tillbaka fitness
public enum Rooms { Normal, Encounter, Special}


public class FitnessFunction : MonoBehaviour
{
    public static Rooms currentRoom;

    [SerializeField]
    private Vector2 bigRoomMultiplier = new Vector2(2,4);
    private List<Node> lbNodes;
    [SerializeField]
    private int encounterFreq = 3;
    private int roomCounter = 0;

    public int RoomCounter
    {
        get { return roomCounter; }
    }
    private Node lbRoot;
    private int lbWidth;
    private int lbHeight;
    private int lbFitness;
    private int turnCounter;
    [SerializeField]
    private int specials = 0;

    public static FitnessFunction Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        lbNodes = new List<Node>();
    }
    /// <summary>
    /// Determines a fitness value for different room variants. And sends those values to EvaluateFitness
    /// </summary>
    /// <param name="nodes"> A list of segments for each room</param>
    /// <param name="root">The root node represents the floor in which the rest of the objects are tied</param>
    /// <returns></returns>
    public bool FitnessFuntion(List<Node> nodes, Node root)
    {
        if (currentRoom == Rooms.Encounter)
        {
            //Debug.Log("==== Encounter Room ====");
            return EvaluateFitness(nodes,5, root, Rooms.Encounter);
        }
        else
        {
            //Debug.Log("**** Roaming Room ****");
            return EvaluateFitness(nodes,3, root, Rooms.Normal);
        }
    }

    public void RoomUpdate()
    {
        if (roomCounter % encounterFreq == 1 && roomCounter > 2)
        {
            currentRoom = Rooms.Encounter;
        }
        else
        {
            currentRoom = Rooms.Normal;
        }
    }

    public int GetRoomFreq()
    {
        return roomCounter % encounterFreq;
    }



    //public bool TimeToTurn()
    //{
    //    if (roomCounter % encounterFreq == 0 && roomCounter > 3)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    /// <summary>
    /// Executes different calculations depending on the room variant.
    /// </summary>
    /// <param name="nodes"> A list of segments for each room</param>
    /// <param name="desiredObjects">Enemies for encounter and weapons for normal</param>
    /// <param name="root">The root node represents the floor in which the rest of the objects are tied</param>
    /// <param name="heightLimit"></param>
    /// <returns></returns>
    public bool EvaluateFitness(List<Node> nodes, int desiredObjects, Node root, Rooms currentRoom)
    {
        int fitnessValue = 0;
        int obstacles = 0;
        specials = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            //Evaluate fitness.
            if(currentRoom == Rooms.Encounter)
            {
                //Give point to traits that are desirable for this room-type
                //Encounter rooms should be...
                if (nodes[i].leaf)
                {
                    BufferMaker(out nodes[i].size.x, out nodes[i].size.y, nodes[i]);
                    fitnessValue = EncounterFitness(nodes[i], root, fitnessValue);
                    obstacles++;
                }
            }
            else
            {
                if (nodes[i].leaf)
                {
                    BufferMaker(out nodes[i].size.x, out nodes[i].size.y, nodes[i]);
                    fitnessValue = StandardFitness(nodes[i], root, fitnessValue);
                    obstacles++;
                }
            }
        }
        if (desiredObjects - 1 <= specials && specials <= desiredObjects + 1)
        {
            //Debug.Log("Sucessesful fitness = " + fitnessValue);
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].leaf)
                {
                    SpawnItemsFromLibrary.Instance.FindClosestKey(nodes[i].size, ProceduralItemLibrary.Instance.itemLibrary);
                    if(nodes[i].size != Vector2.zero)
                    {
                        SpawnItemsFromLibrary.Instance.SpawnItems(nodes[i], root);
                    }
                }
            }
            ResetLastFitness();
            return true;
        }
        else
        {
            if (lbFitness < fitnessValue)
            {
                lbFitness = fitnessValue;
                lbNodes = nodes;
                lbRoot = root;
                lbWidth = (int)root.size.x;
                lbHeight = (int)root.size.y;
            }
        }
        return false;
    }
    /// <summary>
    /// Gives fitness values to desired traits for a "roaming room" outside of combat.
    /// </summary>
    /// <param name="node">A segment of the room</param>
    /// <param name="root">The root node represents the floor in which the rest of the objects are tied</param>
    /// <param name="fitness">The desired fitness value</param>
    /// <returns></returns>
    public int StandardFitness(Node node, Node root, int fitness)
    {
        //if (node.size.x * node.size.y < root.size.x * root.size.y / 200)
        //    fitness -= 20;

        //else if (node.size.x * node.size.y > root.size.x * root.size.y / 100)
        //    fitness += 40;

        //else
        //    fitness += 10;

        if (SpawnItemsFromLibrary.Instance.SeeClosestKey(node).name == "Loot" && SpawnItemsFromLibrary.Instance.SeeClosestKey(node) != null && node.size != Vector2.zero)
        {
            specials++;
            fitness++;
        }

        fitness = TooCloseCheck(node, 20, root, fitness, 400);

        return fitness;
    }

    /// <summary>
    /// Gives fitness values to desired traits for a "encounter room".
    /// </summary>
    /// <param name="node">A segment of the room</param>
    /// <param name="root">The root node represents the floor in which the rest of the objects are tied</param>
    /// <param name="fitness">The fitness value</param>
    /// <returns></returns>
    public int EncounterFitness(Node node, Node root, int fitness)
    {
        //Checks if the nodes are not to small
        if (node.size.x <= root.size.x / 6 && node.size.y <= root.size.y / 6)
        {
            fitness--;
        }
        else
        {
            fitness++;
        }

        //Additional values.
        if (SpawnItemsFromLibrary.Instance.SeeClosestKey(node).name == "Hurdles" && SpawnItemsFromLibrary.Instance.SeeClosestKey(node) != null && node.size != Vector2.zero)
        {
            specials++;
            fitness++;
        }
        fitness = TooCloseCheck(node, 20, root, fitness, 1);
        //Debug.Log("Encounter Fitness : " + fitness);
        return fitness;
    }

    /// <summary>
    /// Checks whether or not objects are too close to an entrance or exit.
    /// </summary>
    /// <param name="node">A segment of the room</param>
    /// <param name="distanceMultiplier"></param>
    /// <param name="root">The root node represents the floor in which the rest of the objects are tied</param>
    /// <param name="fitness">The fitness value</param>
    /// <param name="penalty">A penalty value that will lower the fitness</param>
    /// <returns></returns>
    private int TooCloseCheck(Node node, float distanceMultiplier, Node root, int fitness, int penalty)
    {
        if (Mathf.Abs(node.origin.x - root.origin.x) < root.size.x / distanceMultiplier)
            fitness -= penalty;

        if (Mathf.Abs(node.origin.y - root.origin.y) < root.size.y / distanceMultiplier)
            fitness -= penalty;

        if (Mathf.Abs((node.origin.y - node.size.y) - (root.origin.y - root.size.y)) < root.size.y / distanceMultiplier)
            fitness -= penalty;

        return fitness;
    }
    /// <summary>
    /// If no desired fitness is found after the designated tries, the best one that was found is used.
    /// </summary>
    public void UseBestVariant()
    {
        for (int i = 0; i < lbNodes.Count; i++)
        {
            if (lbNodes[i].leaf)
            {
                BufferMaker(out lbNodes[i].size.x, out lbNodes[i].size.y, lbNodes[i]);
                SpawnItemsFromLibrary.Instance.FindClosestKey(lbNodes[i].size, ProceduralItemLibrary.Instance.itemLibrary);
                if (lbNodes[i].size != Vector2.zero)
                {
                    SpawnItemsFromLibrary.Instance.SpawnItems(lbNodes[i], lbRoot);
                }
            }
        }
        ResetLastFitness();
        //Debug.Log("Last resort was used");
    }
    /// <summary>
    /// Resets to zero the last best fitness for future use.
    /// </summary>
    public void ResetLastFitness()
    {
        lbFitness = 0;
        lbNodes = new List<Node>();
        lbRoot = new Node(Vector2.zero, Vector2.zero);
        lbWidth = 0;
        lbHeight = 0;
    }
    /// <summary>
    /// Creates a buffer for node
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="node"></param>
    private void BufferMaker(out float x, out float y, Node node)
    {
        float bufferX = node.size.x / 16;
        float bufferY = node.size.y / 16;
        x = Random.Range(bufferX, node.size.x - bufferX);
        y = Random.Range(bufferY, node.size.y - bufferY);

    }
    /// <summary>
    /// Gives new room size value to the next room if it is an encounter room.
    /// </summary>
    /// <param name="widthLimits"></param>
    /// <param name="heightLimits"></param>
    /// <param name="size"></param>
    /// <param name="generations">A value that can change the amount of generations of perticular rooms</param>
    /// <returns></returns>
    public Vector2 NextRoomFitness(Vector2 widthLimits, Vector2 heightLimits, Vector2 size, Vector2 oldSize,Vector2 lastSize, int generations)
    {
        size.x = Random.Range((int)widthLimits.x, (int)widthLimits.y);
        size.y = Random.Range((int)heightLimits.x, (int)heightLimits.y);
        if (roomCounter % encounterFreq == 0 && roomCounter > 2)
        {
            generations = generations + 1;
            size.x *= bigRoomMultiplier.x;
            size.y *= bigRoomMultiplier.y;
        }
        else if(roomCounter % encounterFreq == 1 && roomCounter > 2)
        {
            BSPTree.Instance.LastSize = new Vector2(BSPTree.Instance.LastSize.x + oldSize.x - size.x, BSPTree.Instance.LastSize.y);
        }
        roomCounter++;
        ChangeScenary(10);
        return size;
    }

    /// <summary></summary>
    /// <param name="howOften">After how many rooms should we change scenary</param>
    private void ChangeScenary(int howOften)
    {
        if (roomCounter % howOften == 0)
        {
            if (SpawnItemsFromLibrary.currentScenary == Scenary.Rural)
            {
                SpawnItemsFromLibrary.currentScenary = Scenary.City;
            }
            else
            {
                SpawnItemsFromLibrary.currentScenary++;
            }
        }
    }
}
