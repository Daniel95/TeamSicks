﻿using System;
using UnityEngine;

/// <summary>
/// Base input class which contains all input events that the game can listen to.
/// </summary>
public class InputBase : MonoBehaviour {

    public enum TouchStates { Holding, Dragging, TouchDown, None }

    /// <summary>
    /// When the user pressed down.
    /// </summary>
    public static Action<Vector2> DownInputEvent;
    /// <summary>
    /// When the user pressed up.
    /// </summary>
    public static Action<Vector2> UpInputEvent;
    /// <summary>
    /// When the user pressed up within a certain amout of time and without moving too much.
    /// </summary>
    public static Action<Vector2> TapInputEvent;
    /// <summary>
    /// When the user pressed too long for a tap.
    /// </summary>
    public static Action TappedExpiredInputEvent;
    /// <summary>
    /// When the user drags.
    /// </summary>
    public static Action<Vector2> DraggingInputEvent;
    /// <summary>
    /// When the users stops with dragging.
    /// </summary>
    public static Action CancelDragInputEvent;
    /// <summary>
    /// When the users continuesly presses down without moving.
    /// </summary>
    public static Action<Vector2> HoldingInputEvent;
    /// <summary>
    /// When the users pressed up after dragging.
    /// </summary>
    public static Action<Vector2> ReleaseInDirectionInputEvent;

    /// <summary>
    /// The amount of distance the user has to move before it is considered a drag.
    /// </summary>
    [SerializeField] protected float DragTreshhold = 0.1f;
    /// <summary>
    /// The time it takes to go from a tap to holding.
    /// </summary>
    [SerializeField] protected float TimeBeforeTappedExpired = 0.15f;

    /// <summary>
    /// The TouchState the input is in.
    /// </summary>
    protected TouchStates TouchState = TouchStates.None;
    /// <summary>
    /// The input update loop.
    /// </summary>
    protected Coroutine InputUpdateCoroutine;

    /// <summary>
    /// Enables or disables the input loop.
    /// </summary>
    /// <param name="enable"></param>
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