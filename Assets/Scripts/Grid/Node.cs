﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Use for layering
    public Action<NodeObjectType> NodeObjectAddedEvent;
    public Action<NodeObjectType> NodeObjectRemovedEvent;

    public Vector2Int GridPosition { get { return gridPosition; } set { gridPosition = value; } }
    public List<NodeObject> NodeObjects { get { return nodeObjects; } }

    private Vector2Int gridPosition;
    private List<NodeObject> nodeObjects = new List<NodeObject>();

    public void AddNodeObject(NodeObject _nodeObject)
    {
        _nodeObject.ParentNode = this;
        _nodeObject.transform.parent = transform;
        NodeObjects.Add(_nodeObject);
        
        //NodeObjects.IndexOf(_nodeObject);
        int _index = GetLayer((int)_nodeObject.NodeObjectType);
        _nodeObject.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = /*(1000 - 10 * NodeObjects[_index].ParentNode.GridPosition.y) +*/ 1000 * _index;
        
        //End layering

        if (NodeObjectAddedEvent != null)
        {
            NodeObjectAddedEvent(_nodeObject.NodeObjectType);
        }
    }
    
    int GetLayer(int _nodeObjectTypeInt)
    {
        switch (_nodeObjectTypeInt)
        {
            case 0:
            case 2:         
                return 0;
            case 1:
                return 1;
            case 3:
            case 7:
            case 8:
                return 2;
            case 4:
                return 4;
            case 5:
            case 6:
                return 5;
            default:
                return 0;
        }
    }

    public void RemoveNodeObject(NodeObject _nodeObject)
    {
        nodeObjects.Remove(_nodeObject);
        if (NodeObjectRemovedEvent != null)
        {
            Debug.Log("node object removed " + _nodeObject.NodeObjectType);
            NodeObjectRemovedEvent(_nodeObject.NodeObjectType);
        }
    }

    private void OnGUI()
    {
        if(!LevelGrid.Instance.DebugMode) { return; }
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        GUI.TextField(new Rect(screenPosition.x - 20, Screen.height - screenPosition.y - 10, 40, 20), gridPosition.ToString());
    }

}