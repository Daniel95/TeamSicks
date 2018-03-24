using UnityEngine;

public class Scrolling : MonoBehaviour
{

    /// <summary>
    /// This script is used on the camera to add the ability to scroll through the level
    /// </summary>

    [SerializeField] private float scrollSpeed = 1;

    private float maxXBound;
    private float minXBound = 0;

    private void Initialise()
    {
        LevelGrid.Instance.GetSize();
        maxXBound = LevelGrid.Instance.GetSize().x * LevelGrid.Instance.GetStep() / 2;
    }

    private void ScrollCamera(Vector2 _delta)
    {
        float _xTranslation = -_delta.x * scrollSpeed;
        float _nextCameraXPosition = transform.position.x + _xTranslation;

        if (_nextCameraXPosition < minXBound)
        {
            transform.position = new Vector3(minXBound, transform.position.y, transform.position.z);
        }
        else if (_nextCameraXPosition > maxXBound)
        {
            transform.position = new Vector3(maxXBound, transform.position.y, transform.position.z);
        }
        else
        {
            transform.Translate(new Vector2(_xTranslation, 0));
        }

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
