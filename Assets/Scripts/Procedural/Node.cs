using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// <para>
/// Everything within the BSP is made out of nodes
/// </para>
///  <para>
///  Author: Tim & Krisitan
///  
/// </para>
/// </summary>
/// 

// Last Edited: 13/10/2021

public class Node
{
    /// <summary>
    /// What node is this nodes parent
    /// </summary>
    public Node parent;
    /// <summary>
    /// A list of this nodes children. Each nodes can have up to two children
    /// </summary>
    public Node[] children;
    /// <summary>
    /// The gameobject in itself
    /// </summary>
    public GameObject gameObject;
    /// <summary>
    /// Actual size
    /// </summary>
    public Vector2 size;
    /// <summary>
    /// Global position
    /// </summary>
    public Vector2 position;
    /// <summary>
    /// Local origin point. Lower left
    /// </summary>
    public Vector2 origin;
    /// <summary>
    /// Which generation does the node belong to
    /// </summary>
    public int generation;
    /// <summary>
    /// Is this a leaf node, meaning that it does not have children
    /// </summary>
    public bool leaf;


    /// <summary>
    /// Create a Root node
    /// </summary>
    /// <param name="size">
    /// Actual size of the Root
    /// </param>
    /// <param name="lastSize">
    /// The size of the last Root made
    /// </param>
    public Node(Vector2 size, Vector2 lastSize)
    {
        this.size = size;
        parent = null;
        origin = new Vector2(lastSize.x, lastSize.y);
        position = new Vector2(origin.x + (size.x / 2),origin.y + (size.y / 2));
        children = new Node[2];
        leaf = true;
    }

    /// <summary>
    /// Create a smaller node
    /// </summary>
    /// <param name="parent">Root node</param>
    /// <param name="size">Actual size</param>
    /// <param name="origin">Origin spot is in the lower left</param>
    /// <param name="generation">Which generation does this belong to?</param>
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
