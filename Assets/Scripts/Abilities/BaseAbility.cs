using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAbility
{
    protected String UIText;
    protected Sprite UIImage;

	public static Action AbilityPlacedEvent;

    protected int UIIndex;

    public int UIIndexGetSet
    {
        get
        {
            return UIIndex;
        }
        set
        {
            UIIndex = value;
        }
    }

	public virtual void OnGenerate()
	{
	    InputBase.TapInputEvent += PlaceOnGrid;
	    AbilityPlacement.OnInteractedEvent += SetIndex;
	}

    public virtual void OnDestroy()
    {
        InputBase.TapInputEvent -= PlaceOnGrid;
        AbilityPlacement.OnInteractedEvent -= SetIndex;
    }

	public virtual void OnClick()
	{
	    
	}

    public virtual void SetIndex(int _index)
    {

    }

    public String UITextGetter()
    {
        return UIText;
    }

    public Sprite UIImageGetter()
    {
        return UIImage;
    }

	private void Start()
	{
		CallAbilityPlacedEvent();
	}

	public void CallAbilityPlacedEvent()
	{
		
	}

	protected virtual void PlaceOnGrid(Vector2 _screenPosition)
	{
		//EndTurnButton.Instance.ClickStartButton();
		if (AbilityPlacedEvent != null)
		{
			Debug.Log("PlaceOnGrid"); 
			AbilityPlacedEvent();
		}
	}
}