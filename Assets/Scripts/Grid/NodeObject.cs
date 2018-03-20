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

    public void UpdateGridPosition(Vector2Int _position)
    {
        Node _node = LevelGrid.Instance.GetNode(_position);

        parentNode.NodeObjects.Remove(this);
        parentNode = _node;
        parentNode.NodeObjects.Add(this);

        transform.parent = parentNode.transform;

        int _index = _node.NodeObjects.IndexOf(this);
        GetComponentInChildren<SpriteRenderer>().sortingOrder = (1000 - 10 * _node.NodeObjects[_index].ParentNode.GridPosition.y) + 1000 * _index;
    }

}