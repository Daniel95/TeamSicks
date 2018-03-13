﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private void OnInput(Vector2 input)
    {
        if (MovesGenerator.Moves <= 0) { return; }
        if (MovesGenerator.Directions.Contains(input)) { return; }

        transform.Translate(input);
        MovesGenerator.Moves--;

        if (MovesGenerator.Moves <= 0)
        {
            StartMoveButton.Instance.SetEnabled(true);
        }
    }

    private void OnEnable()
    {
        PlayerInput.PlayerInputEvent += OnInput;
    }

    private void OnDisable()
    {
        PlayerInput.PlayerInputEvent -= OnInput;
    }

}
