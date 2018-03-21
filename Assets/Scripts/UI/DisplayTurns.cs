using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DisplayTurns : MonoBehaviour
{
    public static Action<int> UpdateTurnCounter;

    [SerializeField]
    Text turnCounter;

    void UpdateCounter(int _count)
    {
        turnCounter.text = "Turn: " + _count.ToString();
    }

    private void OnEnable()
    {
        UpdateTurnCounter += UpdateCounter;
    }

    private void OnDisable()
    {
        UpdateTurnCounter -= UpdateCounter;
    }
}
