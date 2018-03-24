using System;
using UnityEngine;

public class InputBase : MonoBehaviour {

    [SerializeField] protected float dragTreshhold = 0.1f;
    [SerializeField] protected float timebeforeTappedExpired = 0.15f;

    public static Action CancelDragInputEvent;
    public static Action<Vector2> DraggingInputEvent;
    public static Action HoldingInputEvent;
    public static Action TapInputEvent;
    public static Action<Vector2> ReleaseInDirectionInputEvent;
    public static Action ReleaseInputEvent;
    public static Action TappedExpiredInputEvent;

}