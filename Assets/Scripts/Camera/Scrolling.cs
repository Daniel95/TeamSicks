using UnityEngine;

public class Scrolling : MonoBehaviour
{

    /// <summary>
    /// This script is used on the camera to add the ability to scroll through the level
    /// </summary>

    [SerializeField] private float scrollSpeed = 1;

    private float maximumScrollDistance;
    private float minimumScrollDistance = 0;

    private void Initialise()
    {
        LevelGrid.Instance.GetSize();
        maximumScrollDistance = LevelGrid.Instance.GetSize().x * LevelGrid.Instance.GetStep() / 2;
    }

    private void ScrollCamera(Vector2 _delta)
    {
        if (transform.position.x <= minimumScrollDistance && -_delta.x <= 0) { return; }
        if (transform.position.x >= maximumScrollDistance && -_delta.x >= 0) { return; }

        transform.Translate(new Vector2(-_delta.x * scrollSpeed, transform.position.y));
    }

    private void OnEnable()
    {
        LevelGrid.LevelGridLoadedEvent += Initialise;
        InputBase.DraggingInputEvent += ScrollCamera;
    }

    private void OnDisable()
    {
        LevelGrid.LevelGridLoadedEvent -= Initialise;
        InputBase.DraggingInputEvent -= ScrollCamera;
    }

}
