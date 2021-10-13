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

// Last Edited: 13-10-2021

public class FitnessFunction : MonoBehaviour
{
    float bigRoomMultiplier = 4;
    SpawnItemsFromLibrary itemLibrary;
    List<Node> lbNodes;
    [SerializeField]
    int encounterFreq = 7;
    int roomCounter = 1;
    Node lbRoot;
    int lbWidth;
    int lbHeight;
    int lbHeightLimit;
    int lbFitness;

    private void Start()
    {
        itemLibrary = GetComponent<SpawnItemsFromLibrary>();
        lbNodes = new List<Node>();
    }
    /// <summary>
    /// Determines a fitness value for different room variants. And sends those values to EvaluateFitness
    /// </summary>
    /// <param name="nodes"> A list of segments for each room</param>
    /// <param name="root">The root node represents the floor in which the rest of the objects are tied</param>
    /// <param name="foundSuitableObstacle">A bool that returns whether or not the fitness value was achieved</param>
    /// <param name="heightLimit"></param>
    /// <returns></returns>
    public bool FitnessFuntion(List<Node> nodes, Node root, bool foundSuitableObstacle, int heightLimit)
    {
        if (roomCounter % encounterFreq == 0)
        {
            Debug.Log("==== Encounter Room ====");
            return foundSuitableObstacle = EvaluateFitness(nodes, /*TODO :  Placeholder*/ 3, root, heightLimit);
        }
        else
        {
            Debug.Log("**** Roaming Room ****");
            return foundSuitableObstacle = EvaluateFitness(nodes, /*TODO :  Placeholder*/ 50, root, heightLimit);
        }
    }
    /// <summary>
    /// Executes different calculations depending on the room variant.
    /// </summary>
    /// <param name="nodes"> A list of segments for each room</param>
    /// <param name="desiredFitness">The desired value of the fitness function</param>
    /// <param name="root">The root node represents the floor in which the rest of the objects are tied</param>
    /// <param name="heightLimit"></param>
    /// <returns></returns>
    public bool EvaluateFitness(List<Node> nodes, int desiredFitness, Node root, int heightLimit)
    {
        int fitnessValue = 0;
        int obstacles = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            //Evaluate fitness.
            if(desiredFitness == 3)
            {
                //Give point to traits that are desirable for this room-type
                //Encounter rooms should be...
                if (nodes[i].leaf)
                {
                    fitnessValue = EncounterFitness(nodes[i], root, fitnessValue);
                    obstacles++;
                }
            }
            else
            {
                if (nodes[i].leaf)
                {
                    fitnessValue = StandardFitness(nodes[i], root, fitnessValue);
                    obstacles++;
                }
                Debug.Log("fitness after check = " + fitnessValue);

                if (obstacles > 6)
                {
                    fitnessValue -= 400;
                }
            }
        }

        if (fitnessValue >= desiredFitness)
        {
            Debug.Log("Sucessesful fitness = " + fitnessValue);
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].leaf)
                {
                    BufferMaker(out nodes[i].size.x, out nodes[i].size.y, nodes[i]);
                    itemLibrary.FindClosestKey(nodes[i]);
                    if(nodes[i].size != Vector2.zero)
                    {
                        itemLibrary.SpawnItems(nodes[i], root);
                    }

                    //TODO : Use items from SpawnItemLibrary
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
                lbHeightLimit = heightLimit;
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
        if (node.size.x * node.size.y < root.size.x * root.size.y / 200)
            return fitness -= 20;

        else if (node.size.x * node.size.y > root.size.x * root.size.y / 100)
            return fitness += 40;

        else
            fitness += 10;

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
            return fitness--;
        }
        else
        {
            fitness++;
        }

        //Additional values.
        

        fitness = TooCloseCheck(node, 20, root, fitness, 1);
        Debug.Log("Encounter Fitness : " + fitness);
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
                itemLibrary.FindClosestKey(lbNodes[i]);
                if (lbNodes[i].size != Vector2.zero)
                {
                    itemLibrary.SpawnItems(lbNodes[i], lbRoot);
                }
            }
        }
        ResetLastFitness();
        Debug.Log("Last resort was used");
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
        lbHeightLimit = 0;
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
    public Vector2 NextRoomFitness(Vector2 widthLimits, Vector2 heightLimits, Vector2 size, int generations)
    {
        size.x = Random.Range((int)widthLimits.x, (int)widthLimits.y);
        size.y = Random.Range((int)heightLimits.x, (int)heightLimits.y);
        if (roomCounter % encounterFreq == 0)
        {
            generations = 4;
            size.x *= bigRoomMultiplier;
            size.y *= bigRoomMultiplier;
        }
        generations = 3;
        roomCounter++;
        return size;
    }
}
