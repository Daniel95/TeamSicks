﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnemyNodeObject Handles animations, updates his position, checks if the enemy is done with his turn, uses A* to movearound
/// </summary>
public class EnemyNodeObject : NodeObject
{
	public static Action ReachedEndpointEvent;
    public static Action TurnCompletedEvent;

    public static List<EnemyNodeObject> EnemyNodeObjects = new List<EnemyNodeObject>();

    public int MovesPerTurn { get { return movesPerTurn; } }

	[SerializeField] private int movesPerTurn = 3;
	[SerializeField] private float moveSpeed = 0.01f;

	[SerializeField] private Animator animator;
	[SerializeField] private string walkingAnimatorBoolName = "Moving";

	private bool ReachedEndpoint { get { return GridPosition == endPoint;  } }
    private bool Moving { get { return animator.GetBool(walkingAnimatorBoolName); } set { animator.SetBool(walkingAnimatorBoolName, value); } }

    private Vector2Int endPoint;
    private Coroutine followPathCoroutine;
    private float animatorTransformStartXScale;

	/// <summary>
	/// ActivateAbility(DirectionType, Int) checks if the enemy was hit by a redirectAbility, if so update the path and start the the FollowPath coroutine
	/// </summary>
	/// <param name="_directionType"></param>
	/// <param name="_moveCount"></param>
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

        Moving = false;
        StopCoroutine(followPathCoroutine);
        followPathCoroutine = null;
        CoroutineHelper.Delay(0.1f, () =>
        {
            followPathCoroutine = StartCoroutine(FollowPath(_path, OnFollowPathCompleted));
        });
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
        if (_path.Count <= 0)
        {
            Moving = false;
            if (_onFollowPathCompletedEvent != null)
            {
                _onFollowPathCompletedEvent();
            }
            yield break;
        }

        Moving = true;
        int _pathIndex = 0;
        Vector2Int _targetGridPosition = _path[_pathIndex];
        Vector2 _worldPositionTarget = LevelGrid.Instance.GridToWorldPosition(_path[_pathIndex]);

        if (transform.position.x > _worldPositionTarget.x)
        {
            animator.transform.localScale = new Vector2(animatorTransformStartXScale * -1, animator.transform.localScale.y);
        }
        else
        {
            animator.transform.localScale = new Vector2(animatorTransformStartXScale, animator.transform.localScale.y);
        }

        while (Moving)
        {
            if((Vector2)transform.position != _worldPositionTarget)
            {
                transform.position = Vector2.MoveTowards(transform.position, _worldPositionTarget, moveSpeed);
            }
            else
            {
                UpdateGridPosition(_targetGridPosition);
                _pathIndex++;

                if (_pathIndex >= _path.Count)
                {
                    break;
                }

                _targetGridPosition = _path[_pathIndex];
                _worldPositionTarget = LevelGrid.Instance.GridToWorldPosition(_targetGridPosition);

                if (transform.position.x > _worldPositionTarget.x)
                {
                    animator.transform.localScale = new Vector2(animatorTransformStartXScale * -1, animator.transform.localScale.y);
                }
                else
                {
                    animator.transform.localScale = new Vector2(animatorTransformStartXScale, animator.transform.localScale.y);
                }
            }
            yield return new WaitForFixedUpdate();
        }

        followPathCoroutine = null;

        if (Moving)
        {
            Moving = false;
            if (_onFollowPathCompletedEvent != null)
            {
                _onFollowPathCompletedEvent();
            }
        }
    }

    private void OnFollowPathCompleted()
    {
        if(GridPosition == endPoint)
        {
            if(ReachedEndpointEvent != null)
            {
                ReachedEndpointEvent();
            }
            return;
        }

        bool enemiesAreMoving = EnemyNodeObjects.Exists(x => x.Moving);
        if (!enemiesAreMoving) {
            if(TurnCompletedEvent != null)
            {
                TurnCompletedEvent();
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
        animatorTransformStartXScale = animator.transform.localScale.x;


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