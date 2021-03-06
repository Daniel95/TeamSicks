﻿using UnityEngine;

public class NodeObject : MonoBehaviour
{
    public NodeObjectType NodeObjectType { get { return nodeObjectType; } set { nodeObjectType = value; } }
    public Node ParentNode { get { return parentNode; } set { parentNode = value; } }
    public bool Impassable { get { return impassable; } set { impassable = value; } }
    public Vector2Int GridPosition { get { return parentNode.GridPosition; } }

    private NodeObjectType nodeObjectType;
    private Node parentNode;
    private bool impassable;

    /// <summary>
    /// Update the gridpositions of this 
    /// </summary>
    /// <param name="_position"></param>
    public void UpdateGridPosition(Vector2Int _position)
    {
        parentNode.RemoveNodeObject(this);

        Node _node = LevelGrid.Instance.GetNode(_position);
        _node.AddNodeObject(this);
    }

}