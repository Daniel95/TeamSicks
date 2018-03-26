using System;
using UnityEngine;

public class RedirectAbilityNodeObject : NodeObject
{

    private void OnNodeObjectAdded(NodeObjectType _nodeObjectType)
    {
        if (_nodeObjectType != NodeObjectType.Enemy) { return; }

        NodeObject _nodeObject = ParentNode.NodeObjects.Find(x => x.NodeObjectType == NodeObjectType.Enemy);
        EnemyNodeObject _enemyNodeObject = (EnemyNodeObject)_nodeObject;

        //TEMP HACK
        int directionTypesLength = Enum.GetNames(typeof(DirectionType)).Length;
        DirectionType _directionType = DirectionType.Left;//(DirectionType)UnityEngine.Random.Range(0, directionTypesLength);
        int _moveCount = UnityEngine.Random.Range(2, 4);
        //TEMP HACK

        _enemyNodeObject.ActivateAbility(_directionType, _moveCount);

        Destroy(this);
    }

    private void Start()
    {
        ParentNode.NodeObjectAddedEvent += OnNodeObjectAdded;
    }

    private void OnDestroy()
    {
        ParentNode.NodeObjectAddedEvent -= OnNodeObjectAdded;
    }

}
