using System;
using UnityEngine;

public class InputBase : MonoBehaviour {

    public enum TouchStates { Holding, Dragging, Tapped, None }

    public static Action<Vector2> DownInputEvent;
    public static Action<Vector2> UpInputEvent;
    public static Action<Vector2> TapInputEvent;
    public static Action TappedExpiredInputEvent;
    public static Action<Vector2> DraggingInputEvent;
    public static Action CancelDragInputEvent;
    public static Action<Vector2> HoldingInputEvent;
    public static Action<Vector2> ReleaseInDirectionInputEvent;

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