using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMoveButton : MonoBehaviour
{
	public static Action OnStartMoveButtonClicked;

	[SerializeField] private Button GenerateMovesButton;

	public void ClickStartButton()
	{
		Debug.Log("Start Move Button Clicked");
		if (OnStartMoveButtonClicked != null)
		{
			OnStartMoveButtonClicked();
		}
	}

	private void SetEnabled(bool enabled)
	{
		//Check if button is used or not (Greyed Out)
	}
}