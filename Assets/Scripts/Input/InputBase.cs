using System;
using UnityEngine;

public class InputBase : MonoBehaviour {

    public enum TouchStates { Holding, Dragging, Tapped, None }

    public static Action CancelDragInputEvent;
    public static Action<Vector2> DraggingInputEvent;
    public static Action HoldingInputEvent;
    public static Action<Vector2>TapInputEvent;
    public static Action<Vector2> ReleaseInDirectionInputEvent;
    public static Action ReleaseInputEvent;
    public static Action TappedExpiredInputEvent;

    [SerializeField] protected float DragTreshhold = 0.1f;
    [SerializeField] protected float TimebeforeTappedExpired = 0.15f;

    protected TouchStates TouchState = TouchStates.None;
    protected Coroutine InputUpdateCoroutine;

    public void EnableInput(bool enable)
    {
        if (enable)
        {
            StartInputUpdate();
        }
        else
        {
            if (InputUpdateCoroutine != null)
            {
                StopCoroutine(InputUpdateCoroutine);
                InputUpdateCoroutine = null;
            }
        }
    }

    protected virtual void StartInputUpdate() { }

}