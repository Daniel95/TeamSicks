using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static Action<Vector2> PlayerInputEvent;

    public static PlayerInput Instance { get { return GetInstance(); } }

    private static PlayerInput instance;

    private new bool enabled;

    public void EnableInputs(bool _enable)
    {
        _enable = enabled;
    }

    private void Update()
    {
        if (!enabled) { return; }

        //Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(PlayerInputEvent != null)
            {
                PlayerInputEvent(Vector2.left);
            }
        }
        //Right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (PlayerInputEvent != null)
            {
                PlayerInputEvent(Vector2.right);
            }
        }
        //Up
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (PlayerInputEvent != null)
            {
                PlayerInputEvent(Vector2.up);
            }
        }
        //Down
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (PlayerInputEvent != null)
            {
                PlayerInputEvent(Vector2.down);
            }
        }
    }

    private static PlayerInput GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerInput>();
        }
        return instance;
    }

}
