using UnityEngine;

public class NodeObject : MonoBehaviour
{
    public NodeObjectType NodeObjectType { get { return nodeObjectType; } set { nodeObjectType = value; } }
    public Node ParentNode { get { return parentNode; } set { parentNode = value; } }

    private NodeObjectType nodeObjectType;
    private Node parentNode;
}
