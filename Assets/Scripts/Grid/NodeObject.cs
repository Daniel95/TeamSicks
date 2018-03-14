using UnityEngine;

public class NodeObject : MonoBehaviour
{
    public NodeObjectType NodeObjectType { get { return nodeObjectType; } set { nodeObjectType = value; } }
    public Node ParentNode { get { return parentNode; } set { parentNode = value; } }

    private NodeObjectType nodeObjectType;
    private Node parentNode;

    public void MoveToGridPosition(Vector2 _position)
    {
        Node _node = GameGrid.Instance.GetNode(_position);
        transform.position = _node.transform.position;

        parentNode.NodeObjects.Remove(this);
        parentNode = _node;
        parentNode.NodeObjects.Add(this);
    }
}
