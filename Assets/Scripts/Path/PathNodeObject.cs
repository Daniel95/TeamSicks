using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The NodeObject that the enemy can walk upon.
/// </summary>
public class PathNodeObject : NodeObject
{

    [SerializeField] private List<NodeObjectType> abilityNodeObjectTypes;
    [SerializeField] private SpriteRenderer pathNodeObjectSpriteRenderer;

    [SerializeField] private Sprite normalPathSprite;
    [SerializeField] private Color normalPathColor;

    [SerializeField] private Sprite abilityPathSprite;
    [SerializeField] private Color abilityPathColor;

    private void OnNodeObjectAdded(NodeObjectType _nodeObjectType)
    {
        bool _containsNodeObjectType = abilityNodeObjectTypes.Exists(x => x == _nodeObjectType);
        if (_containsNodeObjectType)
        {
            pathNodeObjectSpriteRenderer.sprite = abilityPathSprite;
            pathNodeObjectSpriteRenderer.color = abilityPathColor;
        }
    }

    private void OnNodeObjectRemoved(NodeObjectType _nodeObjectType)
    {
        bool _containsNodeObjectType = abilityNodeObjectTypes.Exists(x => x == _nodeObjectType);
        if (_containsNodeObjectType)
        {
            pathNodeObjectSpriteRenderer.sprite = normalPathSprite;
            pathNodeObjectSpriteRenderer.color = normalPathColor;
        }
    }

    private void Start()
    {
        ParentNode.NodeObjectAddedEvent += OnNodeObjectAdded;
        ParentNode.NodeObjectRemovedEvent += OnNodeObjectRemoved;

    }

    private void OnDestroy()
    {
        ParentNode.NodeObjectAddedEvent -= OnNodeObjectAdded;
        ParentNode.NodeObjectRemovedEvent -= OnNodeObjectRemoved;
    }

}
