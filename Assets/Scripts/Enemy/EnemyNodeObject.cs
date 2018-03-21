using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNodeObject : NodeObject
{
	public static Action EnemyReachedEndpoint;
    public static Action EnemyTurnCompletedEvent;

    public static List<EnemyNodeObject> EnemyNodeObjects = new List<EnemyNodeObject>();

    private bool ReachedEndpoint { get { return GridPosition == endPoint;  } }
    private bool Moving { get { return moving;  } }

    [SerializeField] private int movesPerTurn = 3;
    [SerializeField] private float moveDelay = 0.4f;

    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip walkingClip;

    private Vector2Int endPoint;
    private bool moving;
    private Coroutine followPathCoroutine;

    public void ActivateAbility(DirectionType _directionType , int _totalmoves)
	{
        Vector2Int _direction = new Vector2Int();

        if (_directionType == DirectionType.Up)
        {
            _direction = Vector2Int.up;
        }
        else if (_directionType == DirectionType.Down)
        {
            _direction = Vector2Int.down;
        }
        else if (_directionType == DirectionType.Left)
        {
            _direction = Vector2Int.left;

        }
        else if (_directionType == DirectionType.Right)
        {
            _direction = Vector2Int.right;
        }

        List<Vector2Int> _path = new List<Vector2Int>();

        for (int i = 1; i < _totalmoves + 1; i++)
        {
            Vector2Int _newGridPosition = GridPosition + _direction * i;
            if (!LevelGrid.Instance.IsImpassable(_newGridPosition) && LevelGrid.Instance.Contains(_newGridPosition))
            {
                _path.Add(_newGridPosition);
            }
            else
            {
                break;
            }
        }

        StopCoroutine(followPathCoroutine);
        followPathCoroutine = StartCoroutine(FollowPath(_path, OnFollowPathCompleted));
	}

    private void StartTurnMovement()
    {
        int[,] impassableMap = LevelGrid.Instance.GetImpassableMap();

        List<Vector2Int> path = AstarHelper.GetPath(impassableMap, GridPosition, endPoint, AstarHelper.AstarPathType.Manhattan);
        path.Remove(GridPosition);
		
        int moves = movesPerTurn < path.Count ? movesPerTurn : path.Count;
        List<Vector2Int> pathThisTurn = path.GetRange(0, moves);

        followPathCoroutine = StartCoroutine(FollowPath(pathThisTurn, OnFollowPathCompleted));
    }

    private IEnumerator FollowPath(List<Vector2Int> _path, Action _onFollowPathCompletedEvent = null)
    {

        moving = true;

        for (int i = 0; i < _path.Count; i++)
        {
            Vector2Int _gridPosition = _path[i];
            Debug.Log("Move to " + _gridPosition);
            MoveToGridPosition(_gridPosition);

            if (i != _path.Count - 1)
            {
                yield return new WaitForSeconds(moveDelay);
            }
        }

        moving = false;

		followPathCoroutine = null;

        if (_onFollowPathCompletedEvent != null)
        {
            _onFollowPathCompletedEvent();
        }
    }

    private void OnFollowPathCompleted()
    {
        if(GridPosition == endPoint)
        {
            if(EnemyReachedEndpoint != null)
            {
                EnemyReachedEndpoint();
            }
            return;
        }

        bool enemiesAreMoving = EnemyNodeObjects.Exists(x => x.Moving);
        if (!enemiesAreMoving) {
            if(EnemyTurnCompletedEvent != null)
            {
                EnemyTurnCompletedEvent();
            }
        }
    }

    private void MoveToGridPosition(Vector2Int _gridPosition)
    {
        UpdateGridPosition(_gridPosition);
        transform.position = ParentNode.transform.position;
    }

    private void ChooseEndpoint()
    {
        if (EndpointNodeObject.Endpoints.Count <= 0)
        {
            Debug.LogError("No EndpointNodeObject to go to in LevelGrid!");
            return;
        }

        int randomEndpointIndex = UnityEngine.Random.Range(0, EndpointNodeObject.Endpoints.Count);
        EndpointNodeObject endpointNodeObject = EndpointNodeObject.Endpoints[randomEndpointIndex];
        endPoint = endpointNodeObject.GridPosition;
    }


    private void Awake()
    {
        EnemyNodeObjects.Add(this);
    }

    private void OnEnable()
    {
        EndTurnButton.PlayerTurnCompletedEvent += StartTurnMovement;
        LevelGrid.LevelGridLoadedEvent += ChooseEndpoint;
    }

    private void OnDisable()
    {
        EndTurnButton.PlayerTurnCompletedEvent -= StartTurnMovement;
        LevelGrid.LevelGridLoadedEvent -= ChooseEndpoint;
    }

}
