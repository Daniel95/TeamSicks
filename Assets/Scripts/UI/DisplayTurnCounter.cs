using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayTurnCounter : MonoBehaviour
{
    public static DisplayTurnCounter Instance { get { return GetInstance(); } }

    private static DisplayTurnCounter instance;

    private static DisplayTurnCounter GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<DisplayTurnCounter>();
        }
        return instance;
    }
    
    [SerializeField]
    Text turnCounterText;

    private int turnCounter = 0;

    public void UpdateTurnCounter()
    {
        turnCounter += 1;
        turnCounterText.text = "Turn: " + turnCounter.ToString();
    }

    private void OnEnable()
    {
        EndTurnButton.PlayerTurnStartedEvent += UpdateTurnCounter;
    }

    private void OnDisable()
    {
        EndTurnButton.PlayerTurnStartedEvent -= UpdateTurnCounter;
    }
}