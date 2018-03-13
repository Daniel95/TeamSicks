using System;
using System.Collections.Generic;
using UnityEngine;

public class MovesGenerator : MonoBehaviour
{
    public static Action MovesGeneratedEvent;

    public static int Moves { get { return moves; } set { moves = value; } }
    public static List<Vector2> Directions { get { return directions; } }

    private static int moves = 0;
    private static List<Vector2> directions = new List<Vector2>();

	[SerializeField] private List<RandomMoveAmount> randomMoves = new List<RandomMoveAmount>();
	[SerializeField] private List<RandomDirection> randomDirections = new List<RandomDirection>();

    private void GeneratedMoves()
    {   
		moves = 0;

		foreach (RandomMoveAmount _moves in randomMoves)
		{
			moves += _moves.GetRandomMoveAmount();
		}
		Debug.Log("Moves: " + moves);

        directions.Clear();

        foreach (RandomDirection _directions in randomDirections)
		{
			Vector2 containsDirections = _directions.GetRandomDirection();
			directions.Add(containsDirections);
			Debug.Log("Direction: " + containsDirections);
		}
	}

    private void OnEnable()
    {
        StartMoveButton.ClickedEvent += GeneratedMoves;
    }

    private void OnDisable()
    {
        StartMoveButton.ClickedEvent -= GeneratedMoves;
    }

}