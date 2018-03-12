using System;
using System.Collections.Generic;
using UnityEngine;

public class MovesGenerator : MonoBehaviour {

	public static Action MovesGeneratedEvent;

	public static int Moves { get { return moves; } }

	public static List<Vector2> Directions { get { return directions; } }

	private static int moves;

	private static List<Vector2> directions = new List<Vector2>();

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
		
	}
}