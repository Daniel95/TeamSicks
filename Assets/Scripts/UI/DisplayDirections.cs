using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDirections : MonoBehaviour {

	public static Action UpdateDirection;

	[SerializeField] private Text directionText;
	private string directionsString = "Directions: ";
	private string horizontalString = "";
	private string verticalString = "";

	private void Awake()
	{
		directionText.text = directionsString + "None";
	}

	private void OnEnable()
	{
		UpdateDirection += DisplayDirectionInput;
	}

	private void OnDisable()
	{
		UpdateDirection -= DisplayDirectionInput;
	}

	private void DisplayDirectionInput()
	{
		if (MovesGenerator.Directions.Contains(Vector2.up)) { verticalString = "Up"; }
		if (MovesGenerator.Directions.Contains(Vector2.down)) { verticalString = "Down"; }
		if (MovesGenerator.Directions.Contains(Vector2.right)) { horizontalString = "Right"; }
		if (MovesGenerator.Directions.Contains(Vector2.left)) { horizontalString = "Left"; }

		directionText.text = directionsString + horizontalString + ", " + verticalString;
	}	
}