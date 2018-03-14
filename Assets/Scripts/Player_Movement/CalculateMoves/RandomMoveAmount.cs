using System;
using UnityEngine;

[Serializable]
public class RandomMoveAmount
{
	[SerializeField] private int maxValue = 6;
	[SerializeField] private int minValue = 1;

	public int GetRandomMoveAmount()
	{
		int totalMoves = UnityEngine.Random.Range(minValue, maxValue);
		return totalMoves; 
	}

}