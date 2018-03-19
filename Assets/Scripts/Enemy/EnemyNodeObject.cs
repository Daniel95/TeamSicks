using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNodeObject : NodeObject
{

    [SerializeField] private int movesPerTurn = 3;
    [SerializeField] private float moveDelay = 0.4f;

    private Vector2Int endPoint;

    private void StartTurnMovement()
    {
        int[,] impassableMap = LevelGrid.Instance.GetImpassableMap();

        List<Vector2Int> path = AstarHelper.GetPath(impassableMap, GridPosition, endPoint, AstarHelper.AstarPathType.Manhattan);
        path.Remove(GridPosition);

        int moves = movesPerTurn < path.Count ? movesPerTurn : path.Count;
        List<Vector2Int> pathThisTurn = path.GetRange(0, moves);
        StartCoroutine(FollowPath(pathThisTurn, () => { EndTurnButton.Instance.SetInteractable(true); }));
    }

    private IEnumerator FollowPath(List<Vector2Int> path, Action OnFollowPathCompletedEvent = null)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int _gridPosition = path[i];
            MoveToGridPosition(_gridPosition);

            if(i != path.Count - 1)
            {
                yield return new WaitForSeconds(moveDelay);
            }
        }

        if (OnFollowPathCompletedEvent != null)
        {
            OnFollowPathCompletedEvent();
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
