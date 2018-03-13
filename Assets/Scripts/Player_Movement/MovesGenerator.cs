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

	[SerializeField] private List<RandomMoves> randomMoves = new List<RandomMoves>();
	[SerializeField] private List<RandomDirections> randomDirections = new List<RandomDirections>();

    private void OnEnable()
    {
        StartMoveButton.ClickedEvent += GeneratedMoves;
    }

    private void OnDisable()
    {
        StartMoveButton.ClickedEvent -= GeneratedMoves;
    }

    private void GeneratedMoves()
    {
		moves = 0;
		foreach (RandomMoves _moves in randomMoves)
		{
			moves += _moves.ReturnTotalMoves();
		}
		Debug.Log("Moves: " + moves);

		foreach (RandomDirections _directions in randomDirections)
		{
			Vector2 containsDirections = _directions.ReturnDirections();
			directions.Add(containsDirections);
			Debug.Log("Direction: " + containsDirections);
		}
	}
}