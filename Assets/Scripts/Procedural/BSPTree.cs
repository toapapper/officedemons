using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BSPTree : MonoBehaviour
{
    Node root;
    public GameObject cubePrefab;
    public List<Node> nodes;
    public List<GameObject> cubes = new List<GameObject>();
    public int generations = 3;
    public int height;
    public int width;
    public int zValue;
    public int counter = 1;
    int missfallMultiplier;
    public int missfallTop = 5;


    public void MakeBSP()
    {
        missfallMultiplier = 0;
        nodes = new List<Node>();
        root = new Node(new Vector2(width, height),counter);
        CreateCube(root);
        BSP(root);
        counter++;
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].leaf)
                CreateRoom(nodes[i]);
        }
        Debug.Log(nodes.Count);
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

        int split = 0;
        int missfall = Random.Range(missfallMultiplier, missfallTop);
        if (missfall == missfallMultiplier && node.generation > 1)
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


            //CreateCube(node1,node);
            //CreateCube(node2,node);


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


            //CreateCube(node1, node);
            //CreateCube(node2, node);


            BSP(node1);
            BSP(node2);
        }
    }

    private void CreateCube(Node node, Node parent)
    {
        GameObject cube = Instantiate(cubePrefab, new Vector3(node.position.x, node.position.y, -node.generation), Quaternion.identity);
        cube.transform.localScale = new Vector3(node.size.x, node.size.y, 1);
        cube.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        cube.gameObject.name = node.generation.ToString();
        node.cube = cube;
        cube.transform.parent = parent.cube.transform;
        cubes.Add(cube);
    }
    private void CreateCube(Node node)
    {
        GameObject cube = Instantiate(cubePrefab, new Vector3(node.position.x, node.position.y, -node.generation), Quaternion.identity);
        cube.transform.localScale = new Vector3(node.size.x, node.size.y, 1);
        cube.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);
        cube.gameObject.name = node.generation.ToString();
        node.cube = cube;
        cubes.Add(cube);
    }

    private void CreateRoom(Node node)
    {
        float x, y;
        BufferMaker(out x, out y, node);
        GameObject cube = Instantiate(cubePrefab, new Vector3(node.position.x, node.position.y, -node.generation), Quaternion.identity);
        cube.transform.localScale = new Vector3(x, y, 1);
        node.cube = cube;
        cube.transform.parent = root.cube.transform;
        cubes.Add(cube);
    }

    private void BufferMaker(out float x, out float y, Node node)
    {
        float bufferX = node.size.x / 4;
        float bufferY = node.size.y / 4;
        x = Random.Range(bufferX, node.size.x - bufferX);
        y = Random.Range(bufferY, node.size.y - bufferY);

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
}
