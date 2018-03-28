using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DisplayTurnCounter : MonoBehaviour
{
    public static Action TurnsOverEvent;

    public static DisplayTurnCounter Instance { get { return GetInstance(); } }

    private static DisplayTurnCounter instance;

    [SerializeField] private int requiredTurnsMultiplier = 3;
    [SerializeField] private Text turnCounterText;

    private int turnsLeftValue = 0;

    public void UpdateTurnCounter()
    {
        turnsLeftValue--;

        if(turnsLeftValue <= 0)
        {
            turnsLeftValue = 0;
            if(TurnsOverEvent != null)
            {
                TurnsOverEvent();
            }
        }

        turnCounterText.text = "Turns left: " + turnsLeftValue.ToString();
    }

    private void CalculateTurns()
    {
        int _longestPath = 0;
        int _highestEnemyMoveCount = 0;
        EnemyNodeObject[] enemyNodeObjects = FindObjectsOfType<EnemyNodeObject>();

        if(enemyNodeObjects.Length <= 0) { return; }

        foreach (EnemyNodeObject _enemyNodeObject in FindObjectsOfType<EnemyNodeObject>())
        {
            if(_enemyNodeObject.MovesPerTurn > _highestEnemyMoveCount)
            {
                _highestEnemyMoveCount = _enemyNodeObject.MovesPerTurn;
            }

            foreach (EndpointNodeObject _endpointNodeObject in FindObjectsOfType<EndpointNodeObject>())
            {
                int[,] _impassableMap = LevelGrid.Instance.GetImpassableMap();
                List<Vector2Int> _path = AstarHelper.GetPath(_impassableMap, _enemyNodeObject.GridPosition, _endpointNodeObject.GridPosition, AstarHelper.AstarPathType.Manhattan);
                if(_path.Count > _longestPath)
                {
                    _longestPath = _path.Count;
                }
            }
        }

        int _turnsRequired = Mathf.CeilToInt(_longestPath / _highestEnemyMoveCount);
        turnsLeftValue = _turnsRequired * requiredTurnsMultiplier;
        turnCounterText.text = "Turns left: " + turnsLeftValue.ToString();
    }

    private void OnEnable()
    {
        EndTurnButton.PlayerTurnCompletedEvent += UpdateTurnCounter;
        LevelGrid.LevelGridLoadedEvent += CalculateTurns;
    }

    private void OnDisable()
    {
        EndTurnButton.PlayerTurnCompletedEvent -= UpdateTurnCounter;
        LevelGrid.LevelGridLoadedEvent += CalculateTurns;
    }

    private static DisplayTurnCounter GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<DisplayTurnCounter>();
        }
        return instance;
    }

}