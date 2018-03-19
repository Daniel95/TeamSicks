using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public Vector2Int GridPosition { get { return gridPosition; } set { gridPosition = value; } }
    public List<NodeObject> NodeObjects { get { return nodeObjects; } set { nodeObjects = value; } }

    private Vector2Int gridPosition;
    private List<NodeObject> nodeObjects = new List<NodeObject>();

}