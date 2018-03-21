using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EndTurnButton : MonoBehaviour
{
	public static Action PlayerTurnCompletedEvent;
    public static Action PlayerTurnStartedEvent;

    public static EndTurnButton Instance { get { return GetInstance(); } }

    private static EndTurnButton instance;

    [SerializeField] private Button startMoveButton;

    private void Start()
    {
        CallPlayerTurnStartedEvent();
    }

	public void ClickStartButton()
	{
		if (PlayerTurnCompletedEvent != null)
		{
			PlayerTurnCompletedEvent();
		}
        SetInteractable(false);
    }

    public void SetInteractable(bool _interactable)
	{
        startMoveButton.interactable = _interactable;
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
        EnemyNodeObject.EnemyTurnCompletedEvent += CallPlayerTurnStartedEvent;
    }

    void OnDisable()
    {
        EnemyNodeObject.EnemyTurnCompletedEvent -= CallPlayerTurnStartedEvent;
    }
}