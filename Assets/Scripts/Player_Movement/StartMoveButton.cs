using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartMoveButton : MonoBehaviour
{
	public static Action ClickedEvent;

    public static StartMoveButton Instance { get { return GetInstance(); } }

    private static StartMoveButton instance;

    [SerializeField] private Button startMoveButton;

	public void ClickStartButton()
	{
		if (ClickedEvent != null)
		{
			ClickedEvent();
		}
		DisplayDirections.UpdateDirection();
		DisplayMoves.UpdateMoves();
        SetInteractable(false);
    }

    public void SetInteractable(bool _interactable)
	{
        startMoveButton.interactable = _interactable;
    }

    private static StartMoveButton GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<StartMoveButton>();
        }
        return instance;
    }

    private void Awake()
    {
        startMoveButton = GetComponent<Button>();
    }

}