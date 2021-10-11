using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node parent;
    public Node[] children;
    public GameObject gameObject;
    public Vector2 size;
    public Vector2 position;
    public Vector2 origin;
    public int generation;
    public bool leaf;

    public Node(Vector2 size, Vector2 lastSize)
    {
        this.size = size;
        parent = null;
        origin = new Vector2(lastSize.x, lastSize.y);
        position = new Vector2(origin.x + (size.x / 2),origin.y + (size.y / 2));
        children = new Node[2];
        leaf = true;
    }

    public Node(Node parent, Vector2 size, Vector2 origin, int generation)
    {
        this.parent = parent;
        this.generation = generation;
        this.origin = origin;
        this.size = size;
        children = new Node[2];
        position = new Vector2(origin.x + (size.x / 2), origin.y + (size.y / 2));
        leaf = true;
    }
}
