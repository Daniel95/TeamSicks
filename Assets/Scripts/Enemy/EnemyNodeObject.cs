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

    private void StartTurnMovement()
    {
        int[,] impassableMap = LevelGrid.Instance.GetImpassableMap();

        List<Vector2Int> path = AstarHelper.GetPath(impassableMap, GridPosition, endPoint, AstarHelper.AstarPathType.Manhattan);
        path.Remove(GridPosition);

        int moves = movesPerTurn < path.Count ? movesPerTurn : path.Count;
        List<Vector2Int> pathThisTurn = path.GetRange(0, moves);
        StartCoroutine(FollowPath(pathThisTurn, () => { OnFollowPathCompletedEvent(); }));
    }

    private IEnumerator FollowPath(List<Vector2Int> path, Action OnFollowPathCompletedEvent = null)
    {
        moving = true;

        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int _gridPosition = path[i];
            MoveToGridPosition(_gridPosition);

            if (i != path.Count - 1)
            {
                yield return new WaitForSeconds(moveDelay);
            }
        }

        moving = false;

        if (OnFollowPathCompletedEvent != null)
        {
            OnFollowPathCompletedEvent();
        }
    }

    private void OnFollowPathCompletedEvent()
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
        if (enemiesAreMoving) {
            Debug.Log("EnemyTurnCompleted");
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
        EndTurnButton.ClickedEvent += StartTurnMovement;
        LevelGrid.LevelGridLoadedEvent += ChooseEndpoint;
    }

    private void OnDisable()
    {
        EndTurnButton.ClickedEvent -= StartTurnMovement;
        LevelGrid.LevelGridLoadedEvent += ChooseEndpoint;
    }

}
