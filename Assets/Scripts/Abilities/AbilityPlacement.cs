using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AbilityPlacement disables and enables both the UIAbilityButtons & Holders. It also removes the AbilityHolder from the mouseCursor on click.
/// Furthermore will the UIAbilities change parents whenever an ability is used.
/// </summary>

public class AbilityPlacement : MonoBehaviour
{
    public static Action<int> OnInteractedEvent;

    public static Action OnEnableButtons;

	[SerializeField] private List<Button> UIAbilityButtons;

	[SerializeField] private List<GameObject> UIAbilityHolders;

	[SerializeField] private Button endTurnButton;

	private GameObject targetAbilityGameObject;
    private Transform abilityHolderParent;

    private Vector2 startPositionVector2;
    private GameObject previousButtonGameObject;

    private bool isInteracting = false;

	/// <summary>
	/// Interact(GameObject) checks if the ability in the UI is clicked an if that is true changeparent. if the ability is placed on the grid RemoveAbilityFromCursor();
	/// </summary>
	/// <param name="_gameObject"></param>
	public void Interact(GameObject _gameObject)
	{
		if (targetAbilityGameObject == null)
		{
			startPositionVector2 = _gameObject.transform.position;
			previousButtonGameObject = _gameObject;
			targetAbilityGameObject = _gameObject;
			isInteracting = true;
			abilityHolderParent = _gameObject.transform.parent;
			targetAbilityGameObject.transform.parent = targetAbilityGameObject.transform.root;
			DisableEndTurnButton();
		}
		else
		{
			RemoveAbilityFromCursor();
		}
	}

	/// <summary>
	/// EnableUIHolderGameobject() sets all the UIAbilityHolders[i] active. 
	/// Calls an Action OnEnableButtons();
	/// </summary>
	public void EnableUIHolderGameobject()
	{
		for (int i = 0; i < UIAbilityHolders.Count; i++)
		{
			UIAbilityHolders[i].SetActive(true);
		}
		OnEnableButtons();
	}

	/// <summary>
	/// DisableUIHolderGameobject() sets all the UIAbilityHolders[i] non-active. 
	/// </summary>
	public void DisableUIHolderGameobject()
	{
		for (int i = 0; i < UIAbilityHolders.Count; i++)
		{
			UIAbilityHolders[i].SetActive(false);
		}
	}

	/// <summary>
	/// DisableUIButtons() sets all the UIAbilityButtons[i] non-active. 
	/// </summary>
	public void DisableUIButtons()
	{
		for (int i = 0; i < UIAbilityButtons.Count; i++)
		{
			UIAbilityButtons[i].enabled = false;
		}
	}

	/// <summary>
	/// EnableUIButtons() sets all the UIAbilityButtons[i] active.
	/// </summary>
	public void EnableUIButtons()
	{
		for (int i = 0; i < UIAbilityButtons.Count; i++)
		{
			UIAbilityButtons[i].enabled = true;
		}
	}

	/// <summary>
	/// SetIndex(int) Activates the Action<int> OnInteractedEvent(int);
	/// </summary>
	/// <param name="_index"></param>
	public void SetIndex(int _index)
	{
		OnInteractedEvent(_index);
	}

	private void Update ()
	{
	    if (targetAbilityGameObject != null)
	    {
	        targetAbilityGameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetAbilityGameObject.transform.position.z);
	    }
	}

    private void OnTapInputEvent(Vector2 _screenPosition)
    {
        if (isInteracting)
        {
            LevelGrid.Instance.ScreenToGridPosition(_screenPosition);
        }
    }

    private void RemoveAbilityFromCursor()
    {
        if (targetAbilityGameObject != null)
        {
            targetAbilityGameObject = null;
            previousButtonGameObject.transform.parent = abilityHolderParent;
            previousButtonGameObject.transform.position = startPositionVector2;
            isInteracting = false;
            EnableEndTurnButton();
        }
    }

    private void DisableEndTurnButton()
    {
        endTurnButton.enabled = false;
    }

    private void EnableEndTurnButton()
    {
        endTurnButton.enabled = true;
    }

    private void OnEnable()
    {
        InputBase.TapInputEvent += OnTapInputEvent;
        RedirectAbility.PlacedOnGridEvent += RemoveAbilityFromCursor;
        RedirectAbility.PlacedOnGridEvent += DisableUIButtons;
        RedirectAbility.PlacedOnGridEvent += DisableUIHolderGameobject;
        EndTurnButton.PlayerTurnStartedEvent += EnableUIButtons;
        EndTurnButton.PlayerTurnStartedEvent += EnableUIHolderGameobject;
    }

    private void OnDisable()
    {
        InputBase.TapInputEvent -= OnTapInputEvent;
        RedirectAbility.PlacedOnGridEvent -= RemoveAbilityFromCursor;
        RedirectAbility.PlacedOnGridEvent -= DisableUIButtons;
        RedirectAbility.PlacedOnGridEvent -= DisableUIHolderGameobject;
        EndTurnButton.PlayerTurnStartedEvent -= EnableUIButtons;
        EndTurnButton.PlayerTurnStartedEvent -= EnableUIHolderGameobject;
    }
}