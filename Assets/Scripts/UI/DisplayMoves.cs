using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMoves : MonoBehaviour {

	public static Action UpdateMoves;

	private string totalMoves = "Total Moves: ";
	[SerializeField] private Text displayMovesText;

	private void Awake()
	{
		displayMovesText.text = totalMoves + MovesGenerator.Moves.ToString();
	}

	private void OnEnable()
	{
		UpdateMoves += DisplayMovesCount;
	}

	private void OnDisable()
	{
		UpdateMoves -= DisplayMovesCount;
	}

	private void DisplayMovesCount()
	{
		displayMovesText.text =	totalMoves + MovesGenerator.Moves.ToString();
	}
}