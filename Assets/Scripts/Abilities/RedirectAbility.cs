using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RedirectAbility : BaseAbility
{
    public static Action PlacedOnGridEvent;

    private const int MIN_MOVE_AMOUNT = 1;
    private const int MAX_MOVE_AMOUNT = 4;
    private Vector2Int gridPosition;

    private DirectionType directionType;
    private int moveAmount;
    private Sprite sprite;

    private int currentIndex = -1;

    public override void OnGenerate()
	{
	    moveAmount = Random.Range(MIN_MOVE_AMOUNT, MAX_MOVE_AMOUNT);

	    UIText = "" + moveAmount;

	    int _randomDirection = Random.Range((int)DirectionType.Up, (int)DirectionType.Left + 1);

	    switch (_randomDirection)
	    {
            case (int)DirectionType.Up:
                UIImage = Resources.Load<Sprite>("RedirectUp");
                sprite = UIImage;
                directionType = DirectionType.Up;
                break;
	        case (int)DirectionType.Down:
                UIImage = Resources.Load<Sprite>("RedirectDown");
	            sprite = UIImage;
                directionType = DirectionType.Down;
                break;
	        case (int)DirectionType.Left:
                UIImage = Resources.Load<Sprite>("RedirectLeft");
	            sprite = UIImage;
                directionType = DirectionType.Left;
                break;
	    }

        base.OnGenerate();
	}

    public override void SetIndex(int _index)
    {
        currentIndex = _index;
    }

    protected override void PlaceOnGrid(Vector2 _screenPosition)
    {
        Vector2Int _gridPosition = LevelGrid.Instance.ScreenToGridPosition(_screenPosition);
        if (currentIndex != UIIndex) { return; }
        if (!LevelGrid.Instance.NodeGrid.ContainsKey(_gridPosition)) { return; }
        if (LevelGrid.Instance.IsImpassable(_gridPosition)) { return; }

        Node _node = LevelGrid.Instance.GetNode(_gridPosition);
        bool _containsEndpoint = _node.NodeObjects.Exists(x => x.NodeObjectType == NodeObjectType.EndPoint);
        if (_containsEndpoint) { return; }

        bool _containsAbility = _node.NodeObjects.Exists(x => x.NodeObjectType == NodeObjectType.RedirectAbility);
        if (_containsAbility) { return; }

        NodeObject _nodeObject =
            LevelGrid.Instance.AddNodeObject(NodeObjectType.RedirectAbility, _gridPosition);
        RedirectAbilityNodeObject _redirectAbilityNodeObject = (RedirectAbilityNodeObject) _nodeObject;

        _redirectAbilityNodeObject.RedirectDirectionType = directionType;
        _redirectAbilityNodeObject.MoveAmount = moveAmount;
        _redirectAbilityNodeObject.NodeObjectSprite = sprite;

        if (PlacedOnGridEvent != null)
        {
            PlacedOnGridEvent();
        }
            
        currentIndex = 0;

        OnDestroy();
    }
}