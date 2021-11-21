using System;
using UnityEngine;

[Serializable]
public class Node
{
    public float hCost;
    public int gCost;
    public int fCost;
    public Vector3 Position;
    public Node ParentNode;
}
