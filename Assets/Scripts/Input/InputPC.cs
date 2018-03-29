using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Input pc handles mobile input logic and the input events of InputBase
/// </summary>
public class InputPC : InputBase
{

    [SerializeField] private KeyCode input = KeyCode.Mouse0;

    private float startDownTime;
    private Coroutine inputUpdateCoroutine;

    /// <summary>
    /// Start the pc input loop.
    /// </summary>
    protected override void StartInputUpdate()
    {
        base.StartInputUpdate();
        InputUpdateCoroutine = StartCoroutine(InputUpdate());
    }

    private IEnumerator InputUpdate()
    {
        Vector2 _lastInputPosition = new Vector2();
        Vector2 _mouseStartPosition = new Vector2();

        while (true)
        {
            if (Input.GetKeyDown(input) && !EventSystem.current.IsPointerOverGameObject())
            {
                TouchState = TouchStates.TouchDown;
                _mouseStartPosition = _lastInputPosition = Input.mousePosition;
                startDownTime = Time.time;

                if (DownInputEvent != null)
                {
                    DownInputEvent(_mouseStartPosition);
                }
            }

            if (TouchState != TouchStates.None)
            {
                if (!Input.GetKeyUp(input))
                {
                    Vector2 _currentMousePosition = Input.mousePosition;

                    if (TouchState == TouchStates.TouchDown)
                    { 
                        if (Time.time - startDownTime > TimeBeforeTappedExpired)
                        {
                            if (TappedExpiredInputEvent != null)
                            {
                                TappedExpiredInputEvent();
                            }
                        }
                    }

                    float _distance = Vector2.Distance(_currentMousePosition, _mouseStartPosition);
                    if (_distance > DragTreshhold)
                    {
                        TouchState = TouchStates.Dragging;

                        Vector2 _delta = _currentMousePosition - _lastInputPosition;
                        if (DraggingInputEvent != null)
                        {
                            DraggingInputEvent(_delta);
                        }
                    }
                    else 
                    {
                        if (TouchState == TouchStates.Dragging)
                        {
                            if (CancelDragInputEvent != null)
                            {
                                CancelDragInputEvent();
                            }
                        }
                        else if(TouchState != TouchStates.TouchDown)
                        {
                            TouchState = TouchStates.Holding;

                            if (HoldingInputEvent != null)
                            {
                                HoldingInputEvent(_currentMousePosition);
                            }
                        }
                    }

                    _lastInputPosition = _currentMousePosition;
                }
                else
                {
                    if (UpInputEvent != null)
                    {
                        UpInputEvent(Input.mousePosition);
                    }

                    if (TouchState == TouchStates.TouchDown)
                    {
                        if (TapInputEvent != null)
                        {
                            TapInputEvent(Input.mousePosition);
                        }
                    }
                    else if (TouchState != TouchStates.Holding)
                    {
                        Vector2 _direction = ((Vector2)Input.mousePosition - _lastInputPosition).normalized;

                        if (ReleaseInDirectionInputEvent != null)
                        {
                            ReleaseInDirectionInputEvent(_direction);
                        }
                    }

                    TouchState = TouchStates.None;
                }
            }

            yield return null;
        }
    }

    private void Awake()
    {
        if (!PlatformHelper.PlatformIsMobile)
        {
            EnableInput(true);
        }
    }

}