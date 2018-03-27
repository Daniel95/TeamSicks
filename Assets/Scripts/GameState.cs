using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    private void OnReachedEndpoint()
    {
        Debug.Log("LOSE");
    }

    private void OnTurnsOverEvent()
    {
        Debug.Log("WIN");
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
