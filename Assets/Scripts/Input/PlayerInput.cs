using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static Action<Vector2Int> PlayerInputEvent;

    public static PlayerInput Instance { get { return GetInstance(); } }

    private static PlayerInput instance;

    private void Update()
    {
        //Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(PlayerInputEvent != null)
            {
                PlayerInputEvent(Vector2Int.left);
            }
        }
        //Right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (PlayerInputEvent != null)
            {
                PlayerInputEvent(Vector2Int.right);
            }
        }
        //Up
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (PlayerInputEvent != null)
            {
                PlayerInputEvent(Vector2Int.up);
            }
        }
        //Down
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (PlayerInputEvent != null)
            {
                PlayerInputEvent(Vector2Int.down);
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
