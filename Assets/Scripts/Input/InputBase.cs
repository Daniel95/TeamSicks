using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBase : MonoBehaviour {

    public static Action RawCancelDragInputEvent;
    public static Action<Vector2> RawDraggingInputEvent;
    public static Action RawHoldingInputEvent;
    public static Action RawTapInputEvent;
    public static Action<Vector2> RawReleaseInDirectionInputEvent;
    public static Action RawReleaseInputEvent;
    public static Action RawTappedExpiredInputEvent;

}