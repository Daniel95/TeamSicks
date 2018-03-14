using UnityEngine;

[RequireComponent(typeof(NodeObject))]
public class PlayerMovement : MonoBehaviour
{

    private NodeObject nodeObject;

    private void OnInput(Vector2 input)
    {
        Debug.Log(input);

        Vector2 nextGridPosition = nodeObject.ParentNode.GridPosition + input;

        Debug.Log(nodeObject.ParentNode.transform.name);
        Debug.Log(nodeObject.ParentNode.GridPosition);
        Debug.Log(input);

        if (MovesGenerator.Moves <= 0 || 
            !MovesGenerator.Directions.Contains(input) ||
            GameGrid.Instance.IsOccupied(nextGridPosition)) { return; }

        nodeObject.UpdateGridPosition(nextGridPosition);
        transform.position = nodeObject.ParentNode.transform.position;
        
        Debug.Log(nodeObject.ParentNode.transform.name);
        Debug.Log(nodeObject.ParentNode.GridPosition);

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
