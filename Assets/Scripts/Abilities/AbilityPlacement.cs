using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPlacement : MonoBehaviour
{

    private GameObject targetAbilityGameObject;

    private Vector2 startPositionVector2;
    private GameObject previousButtonGameObject;
    
	void Update ()
	{
	    if (targetAbilityGameObject != null)
	    {
	        //targetAbilityGameObject.transform.position = Input.mousePosition;
	        targetAbilityGameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetAbilityGameObject.transform.position.z);
	    }
	}

    public void Interact(GameObject _gameObject)
    {
        if (targetAbilityGameObject == null)
        {
            startPositionVector2 = _gameObject.transform.position;
            previousButtonGameObject = _gameObject;
            targetAbilityGameObject = _gameObject;
        }
        else if(targetAbilityGameObject != null)
        {
            targetAbilityGameObject = null;
            previousButtonGameObject.transform.position = startPositionVector2;
        }
    }
}
