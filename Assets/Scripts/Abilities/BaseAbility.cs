using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAbility
{
    protected String UIText;
    protected Sprite UIImage;


	public virtual void OnGenerate()
	{
	    InputBase.TapInputEvent += PlaceOnGrid;
    }

	public virtual void OnClick()
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