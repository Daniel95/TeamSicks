using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAbility : MonoBehaviour
{
	public Button button;
	private bool isSelected;

	public virtual void Ability()
	{

	}

	public virtual void OnClick()
	{
		if(button.IsInteractable())
		{
			this.isSelected = true;
			Debug.Log(isSelected);



			//LevelGrid.Instance.IsImpassable();
			//LevelGrid.Instance.ScreenToGridPosition();
		}
		else if(!button.IsInteractable())
		{
			this.isSelected = false;
			Debug.Log(isSelected);
		}
	}

	private void PlaceOnGrid()
	{

	}


}
