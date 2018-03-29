using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// RedirectAbility places the ability on the grid & generates the different directions the player can use. 
/// </summary>
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

	/// <summary>
	/// OnGenerate() generates the different directions with a random amount. After it calls base.Generate from the BaseAbility class.
	/// </summary>
    public override void OnGenerate()
	{
	    moveAmount = Random.Range(MIN_MOVE_AMOUNT, MAX_MOVE_AMOUNT);

	    UIText = "" + moveAmount;

	    int _randomDirection = Random.Range((int)DirectionType.Up, (int)DirectionType.Right + 1);

	    switch (_randomDirection)
	    {
            case (int)DirectionType.Up:
                UIImage = Resources.Load<Sprite>("arrow_up");
                sprite = UIImage;
                directionType = DirectionType.Up;
                break;
	        case (int)DirectionType.Down:
                UIImage = Resources.Load<Sprite>("arrow_down");
	            sprite = UIImage;
                directionType = DirectionType.Down;
                break;
	        case (int)DirectionType.Left:
                UIImage = Resources.Load<Sprite>("arrow_left");
	            sprite = UIImage;
                directionType = DirectionType.Left;
                break;
	        case (int)DirectionType.Right:
                UIImage = Resources.Load<Sprite>("arrow_right");
	            sprite = UIImage;
                directionType = DirectionType.Right;
                break;
	    }
        base.OnGenerate();
	}

	/// <summary>
	/// SetIndex(int) sets the currentIndex to _index<int>
	/// </summary>
	/// <param name="_index"></param>
    public override void SetIndex(int _index)
    {
        currentIndex = _index;
    }

	/// <summary>
	/// PlaceOnGrid(Vector2) Places the redirectAbility on the grid while checking different conditions.
	/// </summary>
	/// <param name="_screenPosition"></param>
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

        NodeObject _redirectNodeObject =
            LevelGrid.Instance.AddNodeObject(NodeObjectType.RedirectAbility, _gridPosition);

        RedirectAbilityNodeObject _redirectAbilityNodeObject = (RedirectAbilityNodeObject) _redirectNodeObject;

        _redirectAbilityNodeObject.RedirectDirectionType = directionType;
        _redirectAbilityNodeObject.MoveAmount = moveAmount;
        _redirectAbilityNodeObject.NodeObjectSprite = sprite;

        if (PlacedOnGridEvent != null)
        {
            PlacedOnGridEvent();
        }
            
        currentIndex = -1;

        OnDestroy();
	}
}