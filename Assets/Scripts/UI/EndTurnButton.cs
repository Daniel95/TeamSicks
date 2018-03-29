using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the button that is used to end the players turn.
/// </summary>
[RequireComponent(typeof(Button))]
public class EndTurnButton : MonoBehaviour
{
    /// <summary>
    /// The PlayerTurnCompletedEvent is trigger when the EndTurnButton is pressed.
    /// </summary>
	public static Action PlayerTurnCompletedEvent;
    /// <summary>
    /// The PlayerTurnStartedEvent is trigger when the enemy has completed its turn.
    /// </summary>
    public static Action PlayerTurnStartedEvent;

    public static EndTurnButton Instance { get { return GetInstance(); } }

    private static EndTurnButton instance;

    [SerializeField] private Button startMoveButton;

    /// <summary>
    /// The logic that should be executed when the EndTurnButton is pressed.
    /// </summary>
	public void ClickStartButton()
	{
		if (PlayerTurnCompletedEvent != null)
		{
			PlayerTurnCompletedEvent();
		}
        SetInteractable(false);
    }

    /// <summary>
    /// Toggles the interactable state of the EndTurnButton
    /// </summary>
    /// <param name="_interactable"></param>
    public void SetInteractable(bool _interactable)
	{
        startMoveButton.interactable = _interactable;
    }

    private void Start()
    {
        CallPlayerTurnStartedEvent();
    }

    private static EndTurnButton GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<EndTurnButton>();
        }
        return instance;
    }

    private void Awake()
    {
        startMoveButton = GetComponent<Button>();
    }

    private void CallPlayerTurnStartedEvent()
    {
        SetInteractable(true);
        if (PlayerTurnStartedEvent != null)
        {
            PlayerTurnStartedEvent();
        }
    }

    void OnEnable()
    {
        EnemyNodeObject.TurnCompletedEvent += CallPlayerTurnStartedEvent;
    }

    void OnDisable()
    {
        EnemyNodeObject.TurnCompletedEvent -= CallPlayerTurnStartedEvent;
    }
}