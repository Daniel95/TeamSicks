using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Input mobile handles mobile input logic and the input events of InputBase
/// </summary>
public class InputMobile : InputBase
{

    /// <summary>
    /// Starts the mobile input loop.
    /// </summary>
    protected override void StartInputUpdate()
    {
        base.StartInputUpdate();
        InputUpdateCoroutine = StartCoroutine(InputUpdate());
    }

    private IEnumerator InputUpdate()
    {
        Vector2 _lastTouchPosition = new Vector2();
        Vector2 _startTouchPosition = new Vector2();
        float _touchDownTime = 0;

        while (true)
        {
            bool _startedTouching = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
            if (_startedTouching && !EventSystem.current.IsPointerOverGameObject())
            {
                TouchState = TouchStates.TouchDown;
                _startTouchPosition = _lastTouchPosition = Input.GetTouch(0).position;
                _touchDownTime = Time.time;

                if (DownInputEvent != null)
                {
                    DownInputEvent(_startTouchPosition);
                }
            }

            if (TouchState != TouchStates.None)
            {
                if (Input.GetTouch(0).phase != TouchPhase.Ended)
                {
                    Vector2 _currentTouchPosition = Input.GetTouch(0).position;

                    if (TouchState == TouchStates.TouchDown)
                    {
                        float timePassedSinceTouchDown = Time.time - _touchDownTime;
                        if (timePassedSinceTouchDown > TimeBeforeTappedExpired)
                        {
                            if (TappedExpiredInputEvent != null)
                            {
                                TappedExpiredInputEvent();
                            }
                        }
                    }

                    float _distance = Vector2.Distance(_currentTouchPosition, _startTouchPosition);
                    if (_distance > DragTreshhold)
                    {
                        TouchState = TouchStates.Dragging;

                        Vector2 _delta = _currentTouchPosition - _lastTouchPosition;

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
                        else if (TouchState != TouchStates.TouchDown)
                        {
                            TouchState = TouchStates.Holding;
                            if (HoldingInputEvent != null)
                            {
                                HoldingInputEvent(Input.GetTouch(0).position);
                            }
                        }
                    }

                    _lastTouchPosition = _currentTouchPosition;
                }
                else
                {
                    if (UpInputEvent != null)
                    {
                        UpInputEvent(Input.GetTouch(0).position);
                    }

                    if (TouchState == TouchStates.TouchDown)
                    {
                        if (TapInputEvent != null)
                        {
                            TapInputEvent(Input.GetTouch(0).position);
                        }
                    }
                    else if (TouchState == TouchStates.Dragging)
                    {
                        Vector2 direction = (Input.GetTouch(0).position - _startTouchPosition).normalized;
                        if (ReleaseInDirectionInputEvent != null)
                        {
                            ReleaseInDirectionInputEvent(direction);
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
        if (PlatformHelper.PlatformIsMobile)
        {
            EnableInput(true);
        }
    }

}