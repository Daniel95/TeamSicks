using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NodeObject))]
public class EnemyMovement : MonoBehaviour
{

    private NodeObject nodeObject;

    [SerializeField] private int movesPerTurn;

    private void StartTurnMovement()
    {
        int[,] impassableMap = LevelGrid.Instance.GetImpassableMap();

        Vector2Int end = new Vector2Int(19, 3);

        List<Vector2Int> path = AstarHelper.GetPath(impassableMap, nodeObject.GridPosition, end, AstarHelper.AstarPathType.Manhattan);

        List<Vector2Int> pathThisTurn = path.GetRange(0, movesPerTurn);
        Debug.Log(pathThisTurn.Count);
        StartCoroutine(FollowPath(pathThisTurn));
    }

    private IEnumerator FollowPath(List<Vector2Int> path, Action OnFollowPathCompletedEvent = null)
    {
        foreach (Vector2Int _gridPosition in path)
        {
            MoveToGridPosition(_gridPosition);
            yield return new WaitForSeconds(1);
        }

        if (OnFollowPathCompletedEvent != null)
        {
            OnFollowPathCompletedEvent();
        }
    }

    private void MoveToGridPosition(Vector2Int _gridPosition)
    {
        nodeObject.UpdateGridPosition(_gridPosition);
        transform.position = nodeObject.ParentNode.transform.position;
    }

    private void OnInput(Vector2Int input)
    {
        Vector2Int nextGridPosition = nodeObject.ParentNode.GridPosition + input;

        if (MovesGenerator.Moves <= 0 ||
            !MovesGenerator.Directions.Contains(input) ||
            !LevelGrid.Instance.Contains(nextGridPosition) ||
            LevelGrid.Instance.Contains(nextGridPosition, NodeObjectType.Obstacle)) { return; }

        nodeObject.UpdateGridPosition(nextGridPosition);
        transform.position = nodeObject.ParentNode.transform.position;

        MovesGenerator.Moves--;
        DisplayMoves.UpdateMoves();

        if (MovesGenerator.Moves <= 0)
        {
            StartMoveButton.Instance.SetInteractable(true);
        }
    }

    private void Awake()
    {
        nodeObject = GetComponent<NodeObject>();
    }

    private void OnEnable()
    {
        PlayerInput.PlayerInputEvent += OnInput;
        LevelGrid.OnLoadedEvent += StartTurnMovement;
    }

    private void OnDisable()
    {
        LevelGrid.OnLoadedEvent -= StartTurnMovement;
        PlayerInput.PlayerInputEvent -= OnInput;
    }

}
