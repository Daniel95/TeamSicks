using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMobile : InputBase
{ 

    private Coroutine inputUpdateCoroutine;
    private enum TouchStates { Holding, Dragging, Tapped, None }
    private TouchStates _touchState = TouchStates.None;

    public void EnableInput(bool enable)
    {
        if (enable)
        {
            inputUpdateCoroutine = StartCoroutine(InputUpdate());
        }
        else
        {
            if (inputUpdateCoroutine != null)
            {
                StopCoroutine(inputUpdateCoroutine);
                inputUpdateCoroutine = null;
            }
        }
    }

    private IEnumerator InputUpdate()
    {
        Vector2 _startTouchPosition = new Vector2();
        float _touchDownTime = 0;

        while (true)
        {
            bool _startedTouching = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
            if (_startedTouching && !EventSystem.current.IsPointerOverGameObject())
            {
                _touchState = TouchStates.Tapped;
                _startTouchPosition = Input.GetTouch(0).position;
                _touchDownTime = Time.time;
            }

            if (_touchState != TouchStates.None)
            {
                if (Input.GetTouch(0).phase != TouchPhase.Ended)
                {
                    float timePassedSinceTouchDown = Time.time - _touchDownTime;
                    if (timePassedSinceTouchDown > timebeforeTappedExpired)
                    {
                        if (_touchState == TouchStates.Tapped)
                        {
                            if(TappedExpiredInputEvent != null)
                            {
                                TappedExpiredInputEvent();
                            }
                        }

                        Vector2 _currentTouchPosition = Input.GetTouch(0).position;
                        float _distance = Vector2.Distance(_currentTouchPosition, _startTouchPosition);

                        if (_distance > dragTreshhold)
                        {
                            _touchState = TouchStates.Dragging;

                            Vector2 _delta = _currentTouchPosition - _startTouchPosition;

                            if(DraggingInputEvent != null ) {
                                DraggingInputEvent(_delta);
                            }
                        }
                        else if (timePassedSinceTouchDown > timebeforeTappedExpired && _touchState != TouchStates.Holding)
                        {
                            if (_touchState == TouchStates.Dragging)
                            {
                                if(CancelDragInputEvent != null)
                                {
                                    CancelDragInputEvent();
                                }
                            }

                            _touchState = TouchStates.Holding;
                            if(HoldingInputEvent != null)
                            {
                                HoldingInputEvent();
                            }
                        }
                    }
                }
                else
                {
                    if(ReleaseInputEvent != null)
                    {
                        ReleaseInputEvent();
                    }

                    if (_touchState == TouchStates.Dragging)
                    {
                        Vector2 direction = (Input.GetTouch(0).position - _startTouchPosition).normalized;
                        if(ReleaseInDirectionInputEvent != null)
                        {
                            ReleaseInDirectionInputEvent(direction);
                        }

                    }
                    else if (_touchState == TouchStates.Tapped)
                    {
                        if(TapInputEvent != null)
                        {
                            TapInputEvent();
                        }
                    }

                    _touchState = TouchStates.None;
                }
            }

            yield return null;
        }
    }

}