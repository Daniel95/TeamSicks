using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    [SerializeField]
    GameObject winUI, loseUI;

    private void OnReachedEndpoint()
    {
        Debug.Log("LOSE");
        loseUI.SetActive(true);
    }

    private void OnTurnsOverEvent()
    {
        winUI.SetActive(true);
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
