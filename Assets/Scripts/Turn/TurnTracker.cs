using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTracker : MonoBehaviour
{
    public static int TurnCount = 1;

    public static void NextTurn()
    {
        TurnCount += 1;
        DisplayTurns.UpdateTurnCounter(TurnCount);
    }

    private void OnEnable()
    {
        EndTurnButton.PlayerTurnCompletedEvent += NextTurn;
    }

    private void OnDisable()
    {
        EndTurnButton.PlayerTurnCompletedEvent -= NextTurn;
    }
}
