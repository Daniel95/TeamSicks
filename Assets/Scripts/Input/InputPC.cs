using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputPC : InputBase {

    [SerializeField] private KeyCode tapInput = KeyCode.Space;
    [SerializeField] private KeyCode dragInput = KeyCode.Mouse0;

    private enum TouchStates { Holding, Dragging, Tapped, None }

    private TouchStates touchState = TouchStates.None;
    private float startDownTime;
    private Coroutine inputUpdateCoroutine;

    public void Start()
    {
        EnableInput(true);
    }

    public void ResetTouched() {
        CancelDragInputEvent();
    }

    public void EnableInput(bool enable) {
        if (enable) {
            inputUpdateCoroutine = StartCoroutine(InputUpdate());
        } else if (inputUpdateCoroutine != null) {
            StopCoroutine(inputUpdateCoroutine);
            inputUpdateCoroutine = null;
        }
    }

    private IEnumerator InputUpdate() {
        Vector2 _lastInputPosition = new Vector2();
        Vector2 _mouseStartPosition = new Vector2();

        while (true) {
            if (Input.GetKeyDown(tapInput)) {
                if (TapInputEvent != null) {
                    TapInputEvent();
                }
            }

            if (Input.GetKeyDown(dragInput) && !EventSystem.current.IsPointerOverGameObject()) {
                touchState = TouchStates.Tapped;
                _mouseStartPosition = _lastInputPosition = Input.mousePosition;
                startDownTime = Time.time;
            }

            if (touchState != TouchStates.None) {
                if (!Input.GetKeyUp(dragInput)) {
                    if (Time.time - startDownTime > timebeforeTappedExpired) {
                        if (touchState == TouchStates.Tapped) {
                            if(TappedExpiredInputEvent != null)
                            {
                                TappedExpiredInputEvent();
                            }
                        }

                        Vector2 _currentMousePosition = Input.mousePosition;
                        float _distance = Vector2.Distance(_currentMousePosition, _mouseStartPosition);

                        if (_distance > dragTreshhold) {
                            touchState = TouchStates.Dragging;

                            Vector2 _delta = _currentMousePosition - _lastInputPosition;
                            if (DraggingInputEvent != null)
                            {
                                DraggingInputEvent(_delta);
                            }

                            _lastInputPosition = _currentMousePosition;

                        } else if (touchState != TouchStates.Holding) {
                            if (touchState == TouchStates.Dragging) {
                                if(CancelDragInputEvent != null)
                                {
                                    CancelDragInputEvent();
                                }
                            }

                            touchState = TouchStates.Holding;
                            if(HoldingInputEvent != null)
                            {
                                HoldingInputEvent();
                            }
                        }
                    }
                } else { 

                    if(ReleaseInputEvent != null)
                    {
                        ReleaseInputEvent();
                    }

                    if (touchState != TouchStates.Holding) {

                        Vector2 _currentInputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector2 _direction = (_currentInputPosition - _lastInputPosition).normalized;

                        if(ReleaseInDirectionInputEvent != null)
                        {
                            ReleaseInDirectionInputEvent(_direction);
                        }

                        _lastInputPosition = _currentInputPosition;
                    }

                    touchState = TouchStates.None;
                }
            }

            yield return null;
        }
    }
}