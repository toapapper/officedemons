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
        node.cube = quad;
        quad.isStatic = true;
        cubes.Add(quad);
        quad.GetComponent<MeshRenderer>().material = mat;
    }

    public void GenerateObstacles(Node node, Node root, int width, int height, float limit)
    {
        float x, y;
        BufferMaker(out x, out y, node);
        //If they become too small use this
        //if (node.size.x * node.size.y < width * height / 200)
        //    return;

        if (TooCloseCheck(node, 20, root, width, height))
            return;

        yvalue =(int)limit / 20;
        GameObject cube = Instantiate(cubePrefab, new Vector3(node.position.x, yvalue, node.position.y), Quaternion.identity);
        cube.transform.localScale = new Vector3(x, yvalue * 2, y);
        node.cube = cube;
        cube.transform.parent = root.cube.transform;
        cube.isStatic = true;
        cubes.Add(cube);
        transformMesh.GetTexture(cube);
    }
}
