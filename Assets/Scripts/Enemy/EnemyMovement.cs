using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{

    private NodeObject nodeObject;

    [SerializeField] private int moveSpeed;

    private void MoveTowardsTarget()
    {
        //NavMesh.CalculatePath
    }

    private void OnInput(Vector2 input)
    {
        Vector2 nextGridPosition = nodeObject.ParentNode.GridPosition + input;

        if (MovesGenerator.Moves <= 0 ||
            !MovesGenerator.Directions.Contains(input) ||
            !GameGrid.Instance.Contains(nextGridPosition) ||
            GameGrid.Instance.Contains(nextGridPosition, NodeObjectType.Obstacle)) { return; }

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
        TestPath();
    }

    private void OnEnable()
    {
        PlayerInput.PlayerInputEvent += OnInput;
    }

    private void OnDisable()
    {
        PlayerInput.PlayerInputEvent -= OnInput;
    }

//            {1, 0, 0, 0, 0, 0, 0, 0},
//            {0, 2, 0, 0, 0, 0, 0, 0},
//            {0, 0, 3, 1, 0, 0, 0, 0},
//            {0, 0, 4, 1, 0, 0, 0, 0},
//            {0, 0, 5, 1, 0, 0, 0, 0},
//            {1, 0, 1, 6, 7, 0, 0, 0},
//            {1, 0, 1, 0, 0, 0, 0, 0},
//            {1, 1, 1, 1, 1, 1, 0, 0},
//            {1, 0, 1, 0, 0, 0, 0, 0},
//            {1, 0, 1, 2, 0, 0, 0, 0}

    private void TestPath()
    {
        int[,] map = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 1, 1, 1, 0, 0, 0, 0},
            { 0, 0, 0, 1, 0, 0, 0, 0},
            { 0, 0, 0, 1, 0, 0, 0, 0},
            { 0, 0, 0, 1, 0, 0, 0, 0},
            { 1, 0, 0, 0, 0, 0, 0, 0},
            { 1, 0, 0, 0, 0, 0, 0, 0},
            { 1, 1, 0, 1, 1, 1, 0, 0},
            { 1, 0, 1, 0, 0, 0, 0, 0},
            { 1, 0, 1, 0, 0, 0, 0, 0}
        };
        int[] start = new int[2] { 0, 0 };
        int[] end = new int[2] { 2, 3 };
        List<Vector2> path = new AstarHelper(map, start, end, AstarHelper.AstarPathType.Manhattan).Result;

        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log(path[i]);
        }
    }

}
