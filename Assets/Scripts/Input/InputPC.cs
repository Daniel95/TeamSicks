using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputPC : InputBase {

    [SerializeField] private KeyCode jumpInput = KeyCode.Space;
    [SerializeField] private KeyCode aimInput = KeyCode.Mouse0;
    [SerializeField] private float dragTreshhold = 0.1f;
    [SerializeField] private float TimebeforeTappedExpired = 0.15f;

    private enum TouchStates { Holding, Dragging, Tapped, None }

    private TouchStates touchState = TouchStates.None;
    private float startDownTime;
    private Coroutine inputUpdateCoroutine;

    private Vector2 mouseStartPosition;

    public void Start()
    {
        EnableInput(true);
    }

    public void ResetTouched() {
        RawCancelDragInputEvent();
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
        while (true) {
            if (Input.GetKeyDown(jumpInput)) {
                //RawTapInputEvent();
            }

            if (Input.GetKeyDown(aimInput) && !EventSystem.current.IsPointerOverGameObject()) {
                touchState = TouchStates.Tapped;
                mouseStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startDownTime = Time.time;
            }

            if (touchState != TouchStates.None) {
                if (!Input.GetKeyUp(aimInput)) {
                    if (Time.time - startDownTime > TimebeforeTappedExpired) {
                        if (touchState == TouchStates.Tapped) {
                            //RawTappedExpiredInputEvent();
                        }

                        if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseStartPosition) > dragTreshhold) {
                            touchState = TouchStates.Dragging;
                            Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseStartPosition).normalized;
                            RawDraggingInputEvent(direction);

                        } else if (touchState != TouchStates.Holding) {
                            if (touchState == TouchStates.Dragging) {
                                //RawCancelDragInputEvent();
                            }

                            touchState = TouchStates.Holding;
                            //RawHoldingInputEvent();
                        }
                    }
                } else { 
                    //RawReleaseInputEvent();

                    if (touchState != TouchStates.Holding) {
                        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
                        //RawReleaseInDirectionInputEvent(direction);
                    }

                    touchState = TouchStates.None;
                }
            }
            yield return null;
        }
    }

}