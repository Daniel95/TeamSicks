using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMobile : InputBase
{
    [SerializeField] private float TimebeforeTappedExpired = 0.15f;
    [SerializeField] private float minimalDistanceToDrag = 0.75f;

    private Coroutine inputUpdateCoroutine;
    private enum TouchStates { Holding, Dragging, Tapped, None }
    private TouchStates TouchState = TouchStates.None;
    private float touchDownTime;
    private Vector2 startTouchPosition;



    public virtual void ResetTouched()
    {
        //RawCancelDragInputEvent();
    }

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
        while (true)
        {
            bool startedTouching = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
            //TODO Add UI Hover Exception
            if (startedTouching && !EventSystem.current.IsPointerOverGameObject())
            {
                TouchState = TouchStates.Tapped;
                startTouchPosition = Input.GetTouch(0).position;
                touchDownTime = Time.time;
            }

            if (TouchState != TouchStates.None)
            {
                if (Input.GetTouch(0).phase != TouchPhase.Ended)
                {
                    float timePassedSinceTouchDown = Time.time - touchDownTime;
                    if (timePassedSinceTouchDown > TimebeforeTappedExpired)
                    {
                        if (TouchState == TouchStates.Tapped)
                        {
                            //RawTappedExpiredInputEvent();
                        }

                        Vector2 worldCurrentTouchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        Vector2 worldStartTouchPosition = Camera.main.ScreenToWorldPoint(startTouchPosition);
                        float dragDistance = Vector2.Distance(worldStartTouchPosition, worldCurrentTouchPosition);

                        if (dragDistance > minimalDistanceToDrag)
                        {

                            Vector2 direction = (Input.GetTouch(0).position - startTouchPosition).normalized;

                            TouchState = TouchStates.Dragging;
                            RawDraggingInputEvent(direction);

                        }
                        else if (timePassedSinceTouchDown > TimebeforeTappedExpired && TouchState != TouchStates.Holding)
                        {
                            if (TouchState == TouchStates.Dragging)
                            {
                                //RawCancelDragInputEvent();
                            }

                            TouchState = TouchStates.Holding;
                            //RawHoldingInputEvent();
                        }
                    }
                }
                else
                {
                    //RawReleaseInputEvent();

                    if (TouchState == TouchStates.Dragging)
                    {
                        Vector2 direction = (Input.GetTouch(0).position - startTouchPosition).normalized;
                        //RawReleaseInDirectionInputEvent(direction);

                    }
                    else if (TouchState == TouchStates.Tapped)
                    {
                        //RawTapInputEvent();
                    }

                    TouchState = TouchStates.None;
                }
            }

            yield return null;
        }
    }

}