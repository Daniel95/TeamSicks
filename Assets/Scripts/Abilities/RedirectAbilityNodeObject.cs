using UnityEngine;

/// <summary>
/// RedirectAbilityNodeObject places the node sprite and the node text on the right grid position and deletes the image when the enemy walks over it.
/// </summary>
public class RedirectAbilityNodeObject : NodeObject
{
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

	[SerializeField] private SpriteRenderer nodeObjectSpriteRenderer;

	[SerializeField] private TextMesh nodeObjectTextMesh;

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
		LevelGrid.Instance.RemoveNodeObject(NodeObjectType, GridPosition);
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