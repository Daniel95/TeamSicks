using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMoveButton : MonoBehaviour
{
	public static Action ClickedEvent;

    public static StartMoveButton Instance { get { return GetInstance(); } }

    private static StartMoveButton instance;

    [SerializeField] private Button startMoveButton;

	public void ClickStartButton()
	{
		Debug.Log("Start Move Button Clicked");
		if (ClickedEvent != null)
		{
			ClickedEvent();
		}
        SetEnabled(false);
	}

	public void SetEnabled(bool enabled)
	{
		//Check if button is used or not (Greyed Out)
	}

    private static StartMoveButton GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<StartMoveButton>();
        }
        return instance;
    }

}