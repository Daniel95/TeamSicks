using System;
using UnityEngine;
using UnityEngine.UI;

public class RedirectAbilityNodeObject : NodeObject
{
    [SerializeField]
    private SpriteRenderer nodeObjectSpriteRenderer;

    [SerializeField]
    private TextMesh nodeObjectTextMesh;

    public DirectionType RedirectDirectionType
    {
        get
        {
            return redirectDirection;
        }
        set
        {
            redirectDirection = value;
        }
    }

    public Sprite NodeObjectSprite
    {
        get
        {
            return nodeObjectSpriteRenderer.sprite;
        }
        set
        {
            nodeObjectSpriteRenderer.sprite = value;
        }
    }

    public int MoveAmount
    {
        get
        {
            return moveAmount;
        }
        set
        {
            nodeObjectTextMesh.text = "" + value;
            moveAmount = value;
        }
    }

    private DirectionType redirectDirection;
    private int moveAmount;

    private void OnNodeObjectAdded(NodeObjectType _nodeObjectType)
    {
        if (_nodeObjectType != NodeObjectType.Enemy) { return; }
        RedirectEnemy();
    }

    private void RedirectEnemy()
    {
        NodeObject _nodeObject = ParentNode.NodeObjects.Find(x => x.NodeObjectType == NodeObjectType.Enemy);
        EnemyNodeObject _enemyNodeObject = (EnemyNodeObject)_nodeObject;

        _enemyNodeObject.ActivateAbility(redirectDirection, moveAmount);

        Destroy(gameObject);
    }

    private void RemoveAbilityFromNode()
    {

    }

    private void Start()
    {
        ParentNode.NodeObjectAddedEvent += OnNodeObjectAdded;
        transform.GetComponentInChildren<MeshRenderer>().sortingLayerID = SortingLayer.NameToID("Ability");
    }

    private void OnDestroy()
    {
        ParentNode.NodeObjectAddedEvent -= OnNodeObjectAdded;
    }

}
