using UnityEngine;

/// <summary>
/// Manages the game lose and win state.
/// </summary>
public class GameState : MonoBehaviour {

    [SerializeField] private GameObject winUI, loseUI;

    private void OnReachedEndpoint()
    {
        loseUI.SetActive(true);
    }

    private void OnTurnsOverEvent()
    {
        winUI.SetActive(true);
    }

    private void OnEnable()
    {
        DisplayTurnCounter.TurnsOverEvent += OnTurnsOverEvent;
        EnemyNodeObject.ReachedEndpointEvent += OnReachedEndpoint;
    }

    private void OnDisable()
    {
        DisplayTurnCounter.TurnsOverEvent -= OnTurnsOverEvent;
        EnemyNodeObject.ReachedEndpointEvent -= OnReachedEndpoint;
    }

}
