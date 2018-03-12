using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private void OnInput(Vector2 input)
    {

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
