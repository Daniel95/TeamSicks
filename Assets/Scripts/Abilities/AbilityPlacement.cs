using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPlacement : MonoBehaviour
{

    public static Action<int> OnInteractedEvent;

    private GameObject targetAbilityGameObject;
    private Transform abilityHolderParent;

    private Vector2 startPositionVector2;
    private GameObject previousButtonGameObject;

    private bool isInteracting = false;


    [SerializeField]
    private List<Button> UIAbilityButtons;

    [SerializeField]
    private List<GameObject> UIAbilityHolders;


    [SerializeField]
    private Button endTurnButton;


    void Update ()
	{
	    if (targetAbilityGameObject != null)
	    {
	        targetAbilityGameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetAbilityGameObject.transform.position.z);
	    }
	}

    private void OnTapped(Vector2 _screenPosition)
    {
        if (isInteracting)
        {
            LevelGrid.Instance.ScreenToGridPosition(_screenPosition);
        }
    }

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

    public void EnableUIHolderGameobject()
    {
        for (int i = 0; i < UIAbilityHolders.Count; i++)
        {
            UIAbilityHolders[i].SetActive(true);
        }
    }

    public void DisableUIHolderGameobject()
    {
        for (int i = 0; i < UIAbilityHolders.Count; i++)
        {
            UIAbilityHolders[i].SetActive(false);
        }
    }

    public void DisableUIButtons()
    {
        for (int i = 0; i < UIAbilityButtons.Count; i++)
        {
            UIAbilityButtons[i].enabled = false;
        }
    }

    public void EnableUIButtons()
    {
        for (int i = 0; i < UIAbilityButtons.Count; i++)
        {
            UIAbilityButtons[i].enabled = true;
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

    public void SetIndex(int _index)
    {
        OnInteractedEvent(_index);
    }

    private void OnEnable()
    {
        InputBase.TapInputEvent += OnTapped;
        RedirectAbility.PlacedOnGridEvent += RemoveAbilityFromCursor;
        RedirectAbility.PlacedOnGridEvent += DisableUIButtons;
        RedirectAbility.PlacedOnGridEvent += DisableUIHolderGameobject;
        EndTurnButton.PlayerTurnStartedEvent += EnableUIButtons;
        EndTurnButton.PlayerTurnStartedEvent += EnableUIHolderGameobject;

    }

    private void OnDisable()
    {
        InputBase.TapInputEvent -= OnTapped;
        RedirectAbility.PlacedOnGridEvent -= RemoveAbilityFromCursor;
        RedirectAbility.PlacedOnGridEvent -= DisableUIButtons;
        RedirectAbility.PlacedOnGridEvent -= DisableUIHolderGameobject;
        EndTurnButton.PlayerTurnStartedEvent -= EnableUIButtons;
        EndTurnButton.PlayerTurnStartedEvent -= EnableUIHolderGameobject;
    }
}
