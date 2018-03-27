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

        NodeObject _nodeObject = ParentNode.NodeObjects.Find(x => x.NodeObjectType == NodeObjectType.Enemy);
        EnemyNodeObject _enemyNodeObject = (EnemyNodeObject)_nodeObject;

        _enemyNodeObject.ActivateAbility(redirectDirection, moveAmount);

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
