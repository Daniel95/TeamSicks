using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{

    /// <summary>
    /// This script is used on the camera to add the ability to scroll through the level
    /// </summary>

    private float MaximumScrollDistance;
    private float MinimumScrollDistance = 0;

    private float ScrollSpeed = 1;

    private void Initialise()
    {
        LevelGrid.Instance.GetSize();
        MaximumScrollDistance = LevelGrid.Instance.GetSize().x * LevelGrid.Instance.GetStep() / 2;
    }


    private void ScrollCamera(Vector2 _direction)
    {
        Debug.Log(MaximumScrollDistance);
        Debug.Log(-_direction.x);
        if (transform.position.x <= MinimumScrollDistance && -_direction.x <= 0)
        {
            return;
        }
        if (transform.position.x >= MaximumScrollDistance && -_direction.x >= 0)
        {
            return;
        }
        transform.Translate(new Vector2(-_direction.x * ScrollSpeed, transform.position.y));
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
