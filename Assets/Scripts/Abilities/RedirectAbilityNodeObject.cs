using System;
using UnityEngine;

public class RedirectAbilityNodeObject : NodeObject
{

    private void OnNodeObjectAdded(NodeObjectType _nodeObjectType)
    {
        Debug.Log("New element on ability node");

        if (_nodeObjectType != NodeObjectType.Enemy) { return; }

        NodeObject _nodeObject = ParentNode.NodeObjects.Find(x => x.NodeObjectType == NodeObjectType.Enemy);
        EnemyNodeObject _enemyNodeObject = (EnemyNodeObject)_nodeObject;

        //TEMP HACK
        int directionTypesLength = Enum.GetNames(typeof(DirectionType)).Length;
        DirectionType _directionType = (DirectionType)UnityEngine.Random.Range(0, directionTypesLength);
        int _moveCount = UnityEngine.Random.Range(0, 4);
        //TEMP HACK

        Debug.Log("directionTypesLength " + directionTypesLength);
        Debug.Log("_directionType " + _directionType);

        _enemyNodeObject.ActivateAbility(_directionType, _moveCount);
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
