using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectAbility : BaseAbility
{

    private const int MIN_MOVE_AMOUNT = 1;
    private const int MAX_MOVE_AMOUNT = 4;
    private DirectionType directionType;

    public override void OnGenerate()
	{
	    float _moveAmount = Random.Range(MIN_MOVE_AMOUNT, MAX_MOVE_AMOUNT);

	    UIText = "" + _moveAmount;

	    int _randomDirection = Random.Range((int)DirectionType.Up, (int)DirectionType.Left + 1);

	    switch (_randomDirection)
	    {
            case (int)DirectionType.Up:
                UIImage = Resources.Load<Sprite>("RedirectUp");
                break;
	        case (int)DirectionType.Down:
                UIImage = Resources.Load<Sprite>("RedirectDown");
                break;
	        case (int)DirectionType.Left:
                UIImage = Resources.Load<Sprite>("RedirectLeft");
                break;
	    }

        base.OnGenerate();
	}

    protected override void PlaceOnGrid(Vector2 _screenPosition)
    {
        Debug.Log("HALLO");
        Vector2Int _screenToGridPosition = LevelGrid.Instance.ScreenToGridPosition(_screenPosition);
        NodeObject _nodeObject = LevelGrid.Instance.AddNodeObject(NodeObjectType.RedirectAbility, _screenToGridPosition);
        Debug.Log(_screenToGridPosition);
    }

    public override void OnClick()
	{
		base.OnClick();
	}	
}