using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputPC : InputBase {

    [SerializeField] private KeyCode tapInput = KeyCode.Space;
    [SerializeField] private KeyCode dragInput = KeyCode.Mouse0;

    private float startDownTime;
    private Coroutine inputUpdateCoroutine;

    public void ResetTouched() {
        CancelDragInputEvent();
    }

    protected override void StartInputUpdate()
    {
        base.StartInputUpdate();
        InputUpdateCoroutine = StartCoroutine(InputUpdate());
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
                TouchState = TouchStates.Tapped;
                _mouseStartPosition = _lastInputPosition = Input.mousePosition;
                startDownTime = Time.time;
            }

            if (TouchState != TouchStates.None) {
                if (!Input.GetKeyUp(dragInput)) {
                    if (Time.time - startDownTime > TimebeforeTappedExpired) {
                        if (TouchState == TouchStates.Tapped) {
                            if(TappedExpiredInputEvent != null)
                            {
                                TappedExpiredInputEvent();
                            }
                        }

                        Vector2 _currentMousePosition = Input.mousePosition;
                        float _distance = Vector2.Distance(_currentMousePosition, _mouseStartPosition);

                        if (_distance > DragTreshhold) {
                            TouchState = TouchStates.Dragging;

                            Vector2 _delta = _currentMousePosition - _lastInputPosition;
                            if (DraggingInputEvent != null)
                            {
                                DraggingInputEvent(_delta);
                            }

                            _lastInputPosition = _currentMousePosition;

                        } else if (TouchState != TouchStates.Holding) {
                            if (TouchState == TouchStates.Dragging) {
                                if(CancelDragInputEvent != null)
                                {
                                    CancelDragInputEvent();
                                }
                            }

                            TouchState = TouchStates.Holding;
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

                    if (TouchState != TouchStates.Holding) {

                        Vector2 _currentInputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector2 _direction = (_currentInputPosition - _lastInputPosition).normalized;

                        if(ReleaseInDirectionInputEvent != null)
                        {
                            ReleaseInDirectionInputEvent(_direction);
                        }

                        _lastInputPosition = _currentInputPosition;
                    }

                    TouchState = TouchStates.None;
                }
            }

            yield return null;
        }
    }

    private void Awake()
    {
        if(!PlatformHelper.PlatformIsMobile)
        {
            EnableInput(true);
        }
    }

}