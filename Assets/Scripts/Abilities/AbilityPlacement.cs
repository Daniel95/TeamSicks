using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPlacement : MonoBehaviour
{

    private GameObject targetAbilityGameObject;
    private Transform abilityHolderParent;

    private Vector2 startPositionVector2;
    private GameObject previousButtonGameObject;

    private bool isInteracting = false;


    
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
            //LevelGrid.Instance.AddNodeObject();
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
        }
        else if(targetAbilityGameObject != null)
        {
            targetAbilityGameObject = null;
            previousButtonGameObject.transform.parent = abilityHolderParent;
            previousButtonGameObject.transform.position = startPositionVector2;
            isInteracting = false;
        }
    }

    private void OnEnable()
    {
        InputBase.TapInputEvent += OnTapped;
    }

    private void OnDisable()
    {
        InputBase.TapInputEvent -= OnTapped;
    }
}
