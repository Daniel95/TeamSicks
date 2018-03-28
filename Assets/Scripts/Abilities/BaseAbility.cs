using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAbility
{
    protected String UIText;
    protected Sprite UIImage;

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
	    InputBase.DownInputEvent += PlaceOnGrid;
	    AbilityPlacement.OnInteractedEvent += SetIndex;
	}

    public virtual void OnDestroy()
    {
        InputBase.DownInputEvent -= PlaceOnGrid;
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

	protected virtual void PlaceOnGrid(Vector2 _screenPosition)
	{

	}
}