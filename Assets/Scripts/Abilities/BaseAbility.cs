using System;
using UnityEngine;

/// <summary>
/// BaseAbility is the Parent class for other abilities. Some methods in the BaseAbility class will be used by the child classes.
/// BaseAbility does not derrive from MonoBehaviour.
/// </summary>
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

	/// <summary>
	/// OnGenerate() if activated by another script will activated events in the method.
	/// </summary>
	public virtual void OnGenerate()
	{
	    InputBase.TapInputEvent += PlaceOnGrid;
	    AbilityPlacement.OnInteractedEvent += SetIndex;
	}

	/// <summary>
	/// OnDestroy() if activated by another script will de-activated events in the method.
	/// </summary>
	public virtual void OnDestroy()
    {
        InputBase.TapInputEvent -= PlaceOnGrid;
        AbilityPlacement.OnInteractedEvent -= SetIndex;
    }


	/// <summary>
	/// SetIndex(int) is being used by child classes.
	/// </summary>
	/// <param name="_index"></param>
    public virtual void SetIndex(int _index)
    {

    }

	/// <summary>
	/// UITextGetter() returns the UIText<Sprite>; 
	/// </summary>
	/// <returns></returns>
    public String UITextGetter()
    {
        return UIText;
    }

	/// <summary>
	/// UIImageGetter() returns the UIImage<Sprite>
	/// </summary>
	/// <returns></returns>
    public Sprite UIImageGetter()
    {
        return UIImage;
    }

	/// <summary>
	/// PlaceOnGrid(Vector2) is used in child classes to place the ability on the grid.
	/// </summary>
	/// <param name="_screenPosition"></param>
	protected virtual void PlaceOnGrid(Vector2 _screenPosition)
	{

	}
}