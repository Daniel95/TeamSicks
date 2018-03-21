using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{

    /// <summary>
    /// This script is used on the camera to add the ability to scroll through the level
    /// </summary>

    private float MaximumScrollDistance;

    private void Initialise()
    {
        LevelGrid.Instance.GetSize();
        MaximumScrollDistance = LevelGrid.Instance.GetSize().x * LevelGrid.Instance.GetStep();
        Debug.Log(MaximumScrollDistance);
    }


    private void ScrollCamera(Vector2 _direction)
    {
        Debug.Log("ScrollDirection! " + _direction);
    }

    private void OnEnable()
    {
        LevelGrid.LevelGridLoadedEvent += Initialise;
        InputBase.RawDraggingInputEvent += ScrollCamera;
    }

    private void OnDisable()
    {
        LevelGrid.LevelGridLoadedEvent -= Initialise;
        InputBase.RawDraggingInputEvent -= ScrollCamera;
    }
}
