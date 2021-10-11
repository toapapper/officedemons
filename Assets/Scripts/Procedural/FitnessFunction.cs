using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Code by: Kristian & Tim
/// Evaluates the BSP tree after a set of desirable traits that correlates to a fitness value.
/// </summary>
public class FitnessFunction : MonoBehaviour
{
    BSPTree bsp;
    GenerateTerrain generateTerrain;
    SpawnItemsFromLibrary itemLibrary;
    List<Node> lbNodes;
    [SerializeField]
    int encounterFreq = 3;
    int roomCounter = 1;
    Node lbRoot;
    int lbWidth;
    int lbHeight;
    int lbHeightLimit;
    int lbFitness;


    private void Start()
    {
        bsp = GetComponent<BSPTree>();
        generateTerrain = GetComponent<GenerateTerrain>();
        itemLibrary = GetComponent<SpawnItemsFromLibrary>();
        lbNodes = new List<Node>();
    }

    public bool FitnessFuntion(List<Node> nodes, Node root, bool foundSuitableObstacle, int heightLimit)
    {
        //Every third room created is an encounter room.
        if (roomCounter % encounterFreq == 0)
        {
            Debug.Log("==== Encounter Room ====");
            return foundSuitableObstacle = EvaluateFitness(nodes, /*TODO :  Placeholder*/ 32, root, heightLimit);
        }
        else
        {
            Debug.Log("**** Roaming Room ****");
            return foundSuitableObstacle = EvaluateFitness(nodes, /*TODO :  Placeholder*/ 50, root, heightLimit);
        }
    }

    public bool EvaluateFitness(List<Node> nodes, int desiredFitness, Node root, int heightLimit)
    {
        int fitnessValue = 0;
        int obstacles = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            //Evaluate fitness.
            if(desiredFitness == 32)
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

    public int EncounterFitness(Node node, Node root, int fitness)
    {
        if (node.size.x < root.size.x / 4)
            fitness--;
        else
            fitness++;

        if (node.size.y < root.size.y / 4)
            fitness--;
        else
            fitness++;

        fitness = TooCloseCheck(node, 20, root, fitness, 1);
        Debug.Log("Encounter Fitness : " + fitness);
        return fitness;
    }
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

    public void ResetLastFitness()
    {
        lbFitness = 0;
        lbNodes = new List<Node>();
        lbRoot = new Node(Vector2.zero, Vector2.zero);
        lbWidth = 0;
        lbHeight = 0;
        lbHeightLimit = 0;
    }

    private void BufferMaker(out float x, out float y, Node node)
    {
        float bufferX = node.size.x / 16;
        float bufferY = node.size.y / 16;
        x = Random.Range(bufferX, node.size.x - bufferX);
        y = Random.Range(bufferY, node.size.y - bufferY);

    }

    public Vector2 NextRoomFitness(Vector2 widthLimits, Vector2 heightLimits, Vector2 size, int generations)
    {
        size.x = Random.Range((int)widthLimits.x, (int)widthLimits.y);
        size.y = Random.Range((int)heightLimits.x, (int)heightLimits.y);
        if (roomCounter % encounterFreq == 0)
        {
            generations = 4;
            size.x *= 10;
            size.y *= 10;
        }
        generations = 3;
        roomCounter++;
        return size;
    }
}
