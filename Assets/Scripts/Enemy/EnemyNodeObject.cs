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
    private bool Moving { get { return animator.GetBool(walkingAnimatorBoolName); } set { animator.SetBool(walkingAnimatorBoolName, value); } }

    [SerializeField] private int movesPerTurn = 3;
    [SerializeField] private float moveSpeed = 0.01f;

    [SerializeField] private Animator animator;
    [SerializeField] private string walkingAnimatorBoolName = "Moving";

    private Vector2Int endPoint;
    private Coroutine followPathCoroutine;

    public void ActivateAbility(DirectionType _directionType , int _moveCount)
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

        Vector2Int startGridPosition = GridPosition;

        for (int i = 1; i < _moveCount + 1; i++)
        {
            Vector2Int _newGridPosition = startGridPosition + _direction * i;
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
        Moving = true;

        int _pathIndex = 0;
        Vector2 _worldPositionTarget = LevelGrid.Instance.GridToWorldPosition(_path[_pathIndex]);

        while(Moving)
        {
            if((Vector2)transform.position == _worldPositionTarget)
            {
                if(_pathIndex < _path.Count)
                {
                    Vector2Int _gridPosition = _path[_pathIndex];
                    UpdateGridPosition(_gridPosition);
                    _worldPositionTarget = LevelGrid.Instance.GridToWorldPosition(_gridPosition);

                    _pathIndex++;
                }
                else
                {
                    Moving = false;
                    break;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, _worldPositionTarget, moveSpeed);
            }

            yield return new WaitForFixedUpdate();
        }

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
        if(animator == null)
        {
            Debug.LogError("EnemyNodeObject has no Animator assigned");
        }
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