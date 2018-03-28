using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMobile : InputBase
{

    public void ResetTouched()
    {
        CancelDragInputEvent();
    }

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
                TouchState = TouchStates.Tapped;
                _startTouchPosition = _lastTouchPosition = Input.GetTouch(0).position;
                _touchDownTime = Time.time;

                if(DownInputEvent != null)
                {
                    DownInputEvent(_startTouchPosition);
                }
            }

            if (TouchState != TouchStates.None)
            {
                if (Input.GetTouch(0).phase != TouchPhase.Ended)
                {
                    Vector2 _currentTouchPosition = Input.GetTouch(0).position;

                    float timePassedSinceTouchDown = Time.time - _touchDownTime;
                    if (timePassedSinceTouchDown > TimebeforeTappedExpired)
                    {
                        if (TouchState == TouchStates.Tapped)
                        {
                            if (TappedExpiredInputEvent != null)
                            {
                                TappedExpiredInputEvent();
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

                            TouchState = TouchStates.Holding;
                            if (HoldingInputEvent != null)
                            {
                                HoldingInputEvent(Input.GetTouch(0).position);
                            }
                        }

                        _lastTouchPosition = _currentTouchPosition;
                    }
                }
                else
                {
                    if (UpInputEvent != null)
                    {
                        UpInputEvent(Input.GetTouch(0).position);
                    }

                    if (TouchState == TouchStates.Tapped)
                    {
                        if (DownInputEvent != null)
                        {
                            DownInputEvent(Input.GetTouch(0).position);
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