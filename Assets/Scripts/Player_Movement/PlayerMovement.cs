using UnityEngine;

[RequireComponent(typeof(NodeObject))]
public class PlayerMovement : MonoBehaviour
{

    private NodeObject nodeObject;

    private void OnInput(Vector2 input)
    {
        Vector2 gridPosition = nodeObject.ParentNode.GridPosition;

        if (MovesGenerator.Moves <= 0 || 
            !MovesGenerator.Directions.Contains(input) ||
            GameGrid.Instance.IsOccupied(gridPosition)) { return; }

        transform.Translate(input);
        MovesGenerator.Moves--;
		DisplayMoves.UpdateMoves();

        if (MovesGenerator.Moves <= 0)
        {
            StartMoveButton.Instance.SetInteractable(true);
            Debug.Log("Move ended");
        }
    }

    private void Awake()
    {
        nodeObject = GetComponent<NodeObject>();
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
