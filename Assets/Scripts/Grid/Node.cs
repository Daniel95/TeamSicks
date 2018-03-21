using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public Action<NodeObjectType> AddedNodeObjectToNode;
    public Action<NodeObjectType> RemoveNodeObjectToNode;

    public Vector2Int GridPosition { get { return gridPosition; } set { gridPosition = value; } }
    public List<NodeObject> NodeObjects { get { return nodeObjects; } }

    private Vector2Int gridPosition;
    private List<NodeObject> nodeObjects = new List<NodeObject>();

    public void AddNodeObject(NodeObject _nodeObject)
    {
        _nodeObject.ParentNode = this;
        _nodeObject.transform.parent = transform;

        nodeObjects.Add(_nodeObject);
        if(AddedNodeObjectToNode != null)
        {
            AddedNodeObjectToNode(_nodeObject.NodeObjectType);
        }
    }

    public void RemoveNodeObject(NodeObject _nodeObject)
    {
        nodeObjects.Remove(_nodeObject);
        if (RemoveNodeObjectToNode != null)
        {
            RemoveNodeObjectToNode(_nodeObject.NodeObjectType);
        }
    }

}