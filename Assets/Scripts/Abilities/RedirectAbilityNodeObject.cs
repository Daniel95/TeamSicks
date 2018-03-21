public class RedirectAbilityNodeObject : NodeObject
{

    private void OnNodeObjectAdded(NodeObjectType _nodeObjectType)
    {
        if(_nodeObjectType != NodeObjectType.Enemy) { return; }


    }

    private void OnEnable()
    {
        ParentNode.NodeObjectAddedEvent += OnNodeObjectAdded;
    }

    private void OnDisable()
    {
        ParentNode.NodeObjectAddedEvent -= OnNodeObjectAdded;
    }

}
